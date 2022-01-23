import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { openDB, IDBPDatabase } from 'idb';
import { WebSocketMessage, WebsocketService } from './websocket.service';
import * as dayjs from 'dayjs';
import * as utc from 'dayjs/plugin/utc';
import { AppService } from './app.service';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { JournalWorkerSetupHelpComponent } from './components/journal-worker-setup-help/journal-worker-setup-help.component';
import { OVERLAY_DATA } from './injector/overlay-data';
import { JournalWorkerHelpData } from './interfaces/journal-worker-help-data';
import { MatSnackBar } from '@angular/material/snack-bar';

dayjs.extend(utc);

@Injectable({
  providedIn: 'root'
})
export class JournalWorkerService {
  private edVirtualWingDb: IDBPDatabase | null = null;
  private journalWorkerInterval: any;
  public isBrowserSupported = true;
  public requiredUserPermissionsGranted = true;
  private relevantEvents: string[] = [];
  private journalLastDate: dayjs.Dayjs = dayjs.utc();
  public serverSettingsReceived: boolean = false;
  public journalWorkerActive: boolean = false;
  public streamingJournalInOtherInstance: boolean = false;

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly appService: AppService,
    private readonly overlay: Overlay,
    private readonly injector: Injector,
    private readonly snackBar: MatSnackBar
  ) {
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
    this.webSocketService.on("JournalStreamingChanged").subscribe((message: WebSocketMessage<JournalStreamingChangedMessage>) => {
      this.streamingJournalInOtherInstance = message.Data.Status;
    });
    // We want to get some information from the server.
    // Depending on the authentication status, this can take a while. Therefore, we shouldn't place any other essential operations further below.
    this.requestJournalSettings();
  }

  private async requestJournalSettings(): Promise<void> {
    const journalSettings = await this.webSocketService.sendMessageAndWaitForResponse<JournalGetSettingsResponseData>("JournalGetSettings", {});
    if (journalSettings !== null && journalSettings.Success) {
      this.serverSettingsReceived = true;
      this.relevantEvents = journalSettings.Data.Events;
      this.journalLastDate = dayjs(journalSettings.Data.JournalLastEventDate);
      this.streamingJournalInOtherInstance = journalSettings.Data.StreamingJournal;
    }
  }

  private helpOverlayRef: OverlayRef | null = null;

  private showHelp(onlyShowFinalStep: boolean): void {
    this.helpOverlayRef = this.overlay.create({
      positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
      backdropClass: "cdk-overlay-transparent-backdrop",
      hasBackdrop: true,
    });
    const helpData: JournalWorkerHelpData = {
      onlyShowFinalStep: onlyShowFinalStep,
    };
    const injectorData = Injector.create({
      parent: this.injector,
      providers: [
        { provide: OVERLAY_DATA, useValue: helpData },
      ],
    });
    const loadingOverlay = new ComponentPortal(JournalWorkerSetupHelpComponent, null, injectorData);
    this.helpOverlayRef.attach(loadingOverlay);
    this.helpOverlayRef.backdropClick().subscribe(() => {
      this.hideHelp();
    });
  }

  private hideHelp(): void {
    if (this.helpOverlayRef !== null) {
      this.helpOverlayRef.dispose();
      this.helpOverlayRef = null;
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
          this.showHelp(true);
          // We don't, so let's request the permissions from the user.
          directoryReadPermission = await directoryHandle.requestPermission({
            mode: "read",
          });
          this.hideHelp();
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
    this.showHelp(false);
    // Ask the user about the directory with the Elite Dangerous journals
    // This needs to happen after an user interaction (e.g. a button click) for security reasons.
    const directoryHandle = await window.showDirectoryPicker();
    this.hideHelp();
    await this.processDirectoryHandle(directoryHandle);
  }

  private async processDirectoryHandle(directoryHandle: FileSystemDirectoryHandle): Promise<void> {
    const result: JournalAndStatusFileFromDirectoryHandleResult = await this.getJournalAndStatusFileFromDirectoryHandle(directoryHandle);
    const journalFile: FileSystemFileHandleAndFile | null = result.journalFile;
    const statusFile: FileSystemHandle | null = result.statusFile;
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
      this.createJournalWorker(directoryHandle, {
        statusFile,
        journalFile: journalFile.handle,
      });
    }
    else {
      console.error("Journal file or status file not found");
      if (statusFile === null) {
        this.snackBar.open("Required files not found. Make sure you select the correct directory.", "Dismiss", {
          duration: 10000,
        });
      }
      else {
        this.snackBar.open("No recent game journal found. Make sure your game is running and that you selected the correct directory.", "Dismiss", {
          duration: 10000,
        });
      }
      await this.requestDirectoryFromUser();
    }
  }

  private async getJournalAndStatusFileFromDirectoryHandle(directoryHandle: FileSystemDirectoryHandle): Promise<JournalAndStatusFileFromDirectoryHandleResult> {
    const now = new Date().getTime();
    let journalFile: FileSystemFileHandleAndFile | null = null;
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
            let journalFileMaxAge = 600000;
            if (!environment.production) {
              journalFileMaxAge = 60000000;
            }
            if ((now - journalFileEntry.lastModified) <= journalFileMaxAge && (journalFile === null || journalFile.file.lastModified < journalFileEntry.lastModified)) {
              journalFile = {
                handle: entry,
                file: journalFileEntry,
              };
            }
            continue;
          }
          // The status file gives us a lot of information about the player.
          else if (entry.name === "Status.json") {
            statusFile = entry;
          }
        }
      }
    }
    catch (e) {
      console.error(e);
    }
    return {
      journalFile: journalFile,
      statusFile: statusFile,
    };
  }

  private journalInactivityNextCheck: dayjs.Dayjs = dayjs.utc();

  private async createJournalWorker(directoryHandle: FileSystemDirectoryHandle, journalWorkerOptions: JournalWorkerOptions): Promise<void> {
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
    this.appService.setLoading(true);
    await this.journalWorker(journalFile, statusFile);
    this.appService.setLoading(false);
    let journalWorkerRunning = false;
    // Unfortunately, the File Access API doesn't have a tracking functionality. So we need to check the files on a regular basis
    this.journalWorkerInterval = setInterval(async () => {
      if (journalWorkerRunning) {
        return;
      }
      try {
        journalWorkerRunning = true;
        await this.journalWorker(journalFile, statusFile);
        const now = dayjs.utc();
        const diff = now.diff(this.journalLastDate, "second");
        if (diff > 120 && now.isAfter(this.journalInactivityNextCheck)) {
          this.journalInactivityNextCheck = now.add(2, "minute");
          const res = await this.getJournalAndStatusFileFromDirectoryHandle(directoryHandle);
          if (res.journalFile !== null && res.journalFile.handle.name != journalFile.handle.name) {
            console.log("Journal rotation detected. Switchting to new journal", res.journalFile.handle.name);
            journalFile.handle = res.journalFile.handle;
            journalFile.lastModified = 0;
            journalFile.lastPosition = 0;
          }
          else {
            console.log("No journal rotation detected.");
          }
        }
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
      const statusEntry = new TextDecoder().decode(await status.arrayBuffer());
      try {
        // If the file is written just as we read it, the Json parse might fail.
        lines.push(JSON.parse(statusEntry));
        statusFile.lastModified = status.lastModified;
      }
      catch (e) {
        if (!environment.production) {
          console.log(e);
        }
      }
    }
    let journalLastDate = this.journalLastDate;
    if (lines.length > 0) {
      const relevantEntries: JournalEntry[] = [];
      for (const journalEntry of lines) {
        if (this.relevantEvents.includes(journalEntry.event)) {
          const entryTime = dayjs(journalEntry.timestamp);
          const isStatus = journalEntry.event === "Status";
          if (isStatus || entryTime >= journalLastDate) {
            relevantEntries.push(journalEntry);
            if (!isStatus) {
              journalLastDate = entryTime;
            }
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
        const response = await this.webSocketService.sendMessageAndWaitForResponse<JournalSendRequestData>("JournalSend", {
          Entries: relevantEntries,
        });
        if (response === null || !response.Success) {
          console.error("JournalSend failed");
          // It failed, so we stop here and try again in a second.
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

interface JournalGetSettingsResponseData {
  Events: string[];
  JournalLastEventDate: string;
  StreamingJournal: boolean;
}

interface JournalSendRequestData {
}

interface JournalAndStatusFileFromDirectoryHandleResult {
  journalFile: FileSystemFileHandleAndFile | null;
  statusFile: FileSystemFileHandle | null;
}

interface FileSystemFileHandleAndFile {
  handle: FileSystemFileHandle;
  file: File;
}

interface JournalStreamingChangedMessage {
  Status: boolean;
}