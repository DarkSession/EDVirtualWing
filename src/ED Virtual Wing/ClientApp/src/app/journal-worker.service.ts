import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { openDB, IDBPDatabase } from 'idb';
import { WebsocketService } from './websocket.service';
import * as dayjs from 'dayjs';
import { Commander } from './interfaces/commander';

@Injectable({
  providedIn: 'root'
})
export class JournalWorkerService {
  private edVirtualWingDb: IDBPDatabase | null = null;
  private journalWorkerInterval: any;
  public isBrowserSupported = true;
  public requiredUserPermissionsGranted = true;
  private relevantEvents: string[] = [];
  private journalLastDate: dayjs.Dayjs = dayjs();
  public serverSettingsReceived: boolean = false;
  public journalWorkerActive: boolean = false;
  public commander: Commander | null = null;

  // relevant events:
  // Fileheader
  // Music
  // Location

  public constructor(private readonly webSocketService: WebsocketService) {
    this.initialize();
  }

  private async initialize(): Promise<void> {
    if (!environment.production) {
      console.log("JournalWorkerService.initialize");
    }
    if (!window.indexedDB) {
      console.log("This browser doesn't support IndexedDB.");
      this.isBrowserSupported = false;
      return;
    }
    // Check if this browser supports the file system access api
    if (typeof window.showDirectoryPicker !== "function") {
      this.isBrowserSupported = false;
      console.log("window.showDirectoryPicker is not available.");
      console.log(window, Window);
      return;
    }
    // Let us open our IndexedDB database
    this.edVirtualWingDb = await openDB("EdVirtualWing", 1, {
      upgrade: (db) => {
        // If there's no "JournalDirectory" store...
        if (!db.objectStoreNames.contains("JournalDirectory")) {
          // we create it
          db.createObjectStore("JournalDirectory");
          if (!environment.production) {
            console.log("Created JournalDirectory object");
          }
        }
        if (!db.objectStoreNames.contains("JournalFiles")) {
          // we create it
          db.createObjectStore("JournalFiles", { keyPath: "JournalFile" });
          if (!environment.production) {
            console.log("Created JournalFiles object");
          }
        }
      },
    });
    // We want to get some information from the server.
    // Depending on the authentication status, this can take a while. Therefore, we shouldn't place any other essential operations further below.
    const relevantEvents = await this.webSocketService.sendMessageAndWaitForResponse<GetJournalSettingsResponse>("GetJournalSettings", {});
    if (relevantEvents !== null) {
      this.serverSettingsReceived = true;
      this.relevantEvents = relevantEvents.Data.Events;
      this.journalLastDate = dayjs(relevantEvents.Data.JournalLastEventDate);
    }
  }

  // This is the entry point if the user wants to start sending their journal to the server.
  public async startJournalWorker(): Promise<void> {
    if (this.edVirtualWingDb === null) {
      return;
    }
    const transaction = this.edVirtualWingDb.transaction("JournalDirectory", "readonly");
    const journalDirectoryStore = transaction.objectStore("JournalDirectory");
    const directoryHandle: FileSystemDirectoryHandle = await journalDirectoryStore.get(1);
    try {
      if (directoryHandle !== null && directoryHandle !== undefined) {
        // Let's first check if we already have permission to read the directory
        let directoryReadPermission = await directoryHandle.queryPermission({
          mode: "read",
        });
        if (directoryReadPermission !== "granted") {
          // We don't, so let's request the permissions from the user.
          directoryReadPermission = await directoryHandle.requestPermission({
            mode: "read",
          });
        }
        // Check if permissions were granted
        if (directoryReadPermission === "granted") {
          // We have a directory handle and we have the required permissions
          this.processDirectoryHandle(directoryHandle);
          return;
        }
      }
    }
    catch (e) {
      console.error(e);
    }
    // If we don't have a directory handle saved or we don't have the request permission, we request the user to provide us a directory
    this.requestDirectoryFromUser();
  }

  private async requestDirectoryFromUser(): Promise<void> {
    // Ask the user about the directory with the Elite Dangerous journals
    // This needs to happen after an user interaction (e.g. a button click) for security reasons.
    const directoryHandle = await window.showDirectoryPicker();
    await this.processDirectoryHandle(directoryHandle);
  }

  private async processDirectoryHandle(directoryHandle: FileSystemDirectoryHandle): Promise<void> {
    const now = new Date().getTime();
    let journalFile: {
      handle: FileSystemFileHandle;
      file: File
    } | null = null;
    let statusFile: FileSystemHandle | null = null;
    try {
      for await (const entry of directoryHandle.values()) {
        // We are only interested in some files
        if (entry.kind === "file") {
          // Check if the file name matches the format of a game journal.
          if (entry.name.match(/Journal.(\d{12}).(\d{2}).log/)) {
            // If it matches, we request the detail for this file
            const journalFileEntry = await entry.getFile();
            // Now we check if it was modified in the last 10 minutes (10 * 60 * 1000). If it was, it's a recent journal
            // Last we check if we already found a journal file earlier, and if yes, if this file is more recent.
            if ((now - journalFileEntry.lastModified) <= 60000000 && (journalFile === null || journalFile.file.lastModified < journalFileEntry.lastModified)) {
              journalFile = {
                handle: entry,
                file: journalFileEntry,
              };
            }
            continue;
          }
          switch (entry.name) {
            // The status file gives us a lot of information about the player.
            case "Status.json": {
              statusFile = entry;
              break;
            }
          }
        }
      }
    }
    catch (e) {
      console.error(e);
    }
    // If we found all the files, we can continue.
    // Otherwise we might have received a wrong folder.
    if (journalFile !== null && statusFile !== null) {
      if (!environment.production) {
        console.log("Required files found.");
        console.log("Journal file:", journalFile.file.name);
      }
      if (this.edVirtualWingDb !== null) {
        const transaction = this.edVirtualWingDb.transaction("JournalDirectory", "readwrite");
        const journalDirectoryStore = transaction.objectStore("JournalDirectory");
        await journalDirectoryStore.put(directoryHandle, 1);
        transaction.commit();
      }
      this.createJournalWorker({
        statusFile,
        journalFile: journalFile.handle,
      });
    }
    else {
      console.error("Journal file or status file not found");
      await this.requestDirectoryFromUser();
    }
  }

  private async createJournalWorker(journalWorkerOptions: JournalWorkerOptions): Promise<void> {
    const journalFile: JournalFileChangeTracker = {
      handle: journalWorkerOptions.journalFile,
      lastModified: 0,
      lastPosition: 0,
    };
    const statusFile: FileChangeTracker = {
      handle: journalWorkerOptions.statusFile,
      lastModified: 0,
    };

    if (this.edVirtualWingDb !== null) {
      const transaction = this.edVirtualWingDb.transaction("JournalDirectory", "readonly");
      const journalDirectoryStore = transaction.objectStore("JournalDirectory");
      const journalLastPosition = await journalDirectoryStore.get(journalWorkerOptions.journalFile.name);
      if (journalLastPosition !== null && typeof journalLastPosition === 'number') {
        journalFile.lastPosition = journalLastPosition;
      }
    }
    if (this.journalWorkerInterval) {
      clearInterval(this.journalWorkerInterval);
    }
    let journalWorkerRunning = false;
    // Unfortunately, the File Access API doesn't have a tracking functionality. So we need to check the files on a regular basis
    this.journalWorkerInterval = setInterval(async () => {
      if (journalWorkerRunning) {
        return;
      }
      try {
        journalWorkerRunning = true;
        await this.journalWorker(journalFile, statusFile);
      }
      catch (e) {
        console.error(e);
      }
      finally {
        journalWorkerRunning = false;
      }
    }, 1000);
    this.journalWorkerActive = true;
  }

  private async journalWorker(journalFile: JournalFileChangeTracker, statusFile: FileChangeTracker): Promise<void> {
    const lines: JournalEntry[] = [];
    // We request the metadata of the journal file
    const journal: File = await journalFile.handle.getFile();
    let lastPosition = journalFile.lastPosition;
    // Then we check if it has been modified since we last checked
    if (journal.lastModified > journalFile.lastModified) {
      // We only read the new data
      const newJournalDataBlob: Blob = journal.slice(lastPosition);
      const newJournalDataStream = newJournalDataBlob.stream();
      const newJournalDataStreamReader = newJournalDataStream.getReader();
      let currentLinePartial: number[] = [];
      while (true) {
        const journalRead = await newJournalDataStreamReader.read();
        if (journalRead.value) {
          const journalPart: Uint8Array = journalRead.value;
          for (let i = 0; i < journalPart.length; i++) {
            const char = journalPart[i];
            currentLinePartial.push(char);
            // We are reading all characters until we find a line break
            if (char === 10) {
              // Once we finished reading the line, we convert the entire line into text
              const line: string = new TextDecoder().decode(new Uint8Array(currentLinePartial));
              // Then convert it to an object
              lines.push(JSON.parse(line));
              // Lastly update the last position and extend it by the number of bytes we just read
              lastPosition += currentLinePartial.length;
              currentLinePartial = [];
            }
          }
        }
        if (journalRead.done) {
          break;
        }
      }
    }
    // We request the metadata of the status file
    const status: File = await statusFile.handle.getFile();
    if (status.lastModified > statusFile.lastModified) {
      statusFile.lastModified = status.lastModified;
      const statusEntry = new TextDecoder().decode(await status.arrayBuffer());
      lines.push(JSON.parse(statusEntry));
    }
    let journalLastDate = this.journalLastDate;
    if (lines.length > 0) {
      const relevantEntries: JournalEntry[] = [];
      for (const journalEntry of lines) {
        if (this.relevantEvents.includes(journalEntry.event)) {
          const entryTime = dayjs(journalEntry.timestamp);
          if (entryTime >= journalLastDate) {
            relevantEntries.push(journalEntry);
            journalLastDate = entryTime;
          }
          else if (!environment.production) {
            console.log(`Removed event '${journalEntry.event}' since it older than the last journal date. The event is from ${entryTime} while the most recent journal entry was from ${journalLastDate}`);
          }
        }
        else if (!environment.production) {
          console.log(`Removed event '${journalEntry.event}' since it is not relevant.`);
        }
      }
      if (relevantEntries.length > 0) {
        if (!environment.production) {
          console.log(`${relevantEntries.length} relevant events`);
        }
        const response = await this.webSocketService.sendMessageAndWaitForResponse<SendJournalResponse>("SendJournal", {
          Entries: relevantEntries,
        });
        if (response?.Data.Commander) {
          this.commander = response?.Data.Commander;
        }
        else {
          // It probably failed, so...
          return;
        }
      }
    }
    if (!environment.production) {
      console.log("journalWorker completed. Updating metadata");
      console.log("journalFile.lastModified: ", journal.lastModified);
      console.log("journalFile.lastPosition: ", lastPosition);
      console.log("journalLastDate: ", journalLastDate.toISOString());
    }
    // After everything is done, we save the last modified timestamp and the last position of the current gamejournal file.
    journalFile.lastModified = journal.lastModified;
    journalFile.lastPosition = lastPosition;
    this.journalLastDate = journalLastDate;
    if (this.edVirtualWingDb !== null) {
      const transaction = this.edVirtualWingDb.transaction("JournalDirectory", "readwrite");
      const journalDirectoryStore = transaction.objectStore("JournalDirectory");
      journalDirectoryStore.put(lastPosition, journalFile.handle.name);
      transaction.commit();
    }
  }
}

interface JournalWorkerOptions {
  journalFile: FileSystemFileHandle;
  statusFile: FileSystemFileHandle;
}

interface FileChangeTracker {
  handle: FileSystemFileHandle;
  lastModified: number;
}

interface JournalFileChangeTracker extends FileChangeTracker {
  lastPosition: number;
}

interface JournalEntry {
  timestamp: string;
  event: string;
}

interface GetJournalSettingsResponse {
  Events: string[];
  JournalLastEventDate: string;
}

interface SendJournalResponse {
  Commander: Commander;
}