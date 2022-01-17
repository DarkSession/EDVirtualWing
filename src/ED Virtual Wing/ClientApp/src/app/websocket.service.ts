import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Guid } from "guid-typescript";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  public connectionStatus: ConnectionStatus = ConnectionStatus.Connecting;
  private initalizeTimeout: any | null = null;
  private webSocket: WebSocket | null = null;
  private messageQueue: WebSocketMessageQueueItem[] = [];
  private responseCallbacks: {
    [key: string]: (response: WebSocketMessage | null) => void;
  } = {};

  public constructor() {
    this.initalize();
  }

  public reconnect(): void {
    if (this.initalizeTimeout != null) {
      clearTimeout(this.initalizeTimeout);
      this.initalizeTimeout = null;
    }
    this.initalize();
  }

  private initalize(): void {
    this.failCallbacks();
    this.connectionStatus = ConnectionStatus.Connecting;
    this.webSocket = new WebSocket(((window.location.protocol == "http:") ? "ws://" : "wss://") + window.location.hostname + ":" + environment.backendPort + "/ws");
    this.webSocket.onopen = () => {
      if (!environment.production) {
        console.log("WebSocket.onopen");
      }
    };
    this.webSocket.onclose = (event: CloseEvent) => {
      if (!environment.production) {
        console.log("WebSocket.onclose", event);
      }
      if (!event.wasClean) {
        if (this.initalizeTimeout != null) {
          clearTimeout(this.initalizeTimeout);
        }
        this.initalizeTimeout = setTimeout(() => {
          this.initalize();
        }, 10000);
        if (!environment.production) {
          console.log("Unclean close. Scheduling another connection in 10s.");
        }
      }
      if (this.connectionStatus != ConnectionStatus.NotAuthenticated) {
        this.connectionStatus = ConnectionStatus.NoConnection;
        this.failCallbacks();
      }
    };
    this.webSocket.onerror = (event: Event) => {
      if (!environment.production) {
        console.log("WebSocket.onerror", event);
      }
    };
    this.webSocket.onmessage = (event: MessageEvent) => {
      if (!environment.production) {
        console.log("WebSocket.onmessage", event);
      }
      const message: WebSocketMessage = JSON.parse(event.data);
      this.processMessage(message);
    };
  }

  private failCallbacks(): void {
    for (const key in this.responseCallbacks) {
      this.responseCallbacks[key](null);
    }
    this.responseCallbacks= {};
  }

  private async processMessage(message: WebSocketMessage): Promise<void> {
    switch (message.Name) {
      case "Authentication": {
        const authenticationData: WebSocketMessageAuthenticationData = message.Data as any;
        if (authenticationData.IsAuthenticated) {
          this.connectionStatus = ConnectionStatus.Authenticated;
          for (const queueItem of this.messageQueue) {
            this.sendMessageInternal(queueItem.message, queueItem.callback);
          }
          this.messageQueue = [];
        }
        else {
          this.connectionStatus = ConnectionStatus.NotAuthenticated;
        }
        break;
      }
      default: {
        if (message.MessageId && this.responseCallbacks[message.MessageId]) {
          const callback = this.responseCallbacks[message.MessageId];
          delete this.responseCallbacks[message.MessageId];
          callback(message);
        }
        else {
          console.info("Unprocessed message", message);
        }
      }
    }
  }

  public sendMessage(name: string, data: any): void {
    const message: WebSocketMessage = {
      Name: name,
      Data: data,
    };
    this.sendMessageInternal(message);
  }

  public sendMessageAndWaitForResponse<T>(name: string, data: any): Promise<WebSocketMessage<T> | null> {
    const message: WebSocketMessage = {
      Name: name,
      Data: data,
      MessageId: Guid.create().toString(),
    };
    let messageResolve;
    const result: Promise<WebSocketMessage<T> | null> = new Promise((resolve) => { messageResolve = resolve; });
    this.sendMessageInternal(message, messageResolve);
    return result;
  }

  private sendMessageInternal(message: WebSocketMessage, callback?: (response: WebSocketMessage | null) => void): void {
    if (this.connectionStatus == ConnectionStatus.Authenticated && this.webSocket != null) {
      if (callback && message.MessageId) {
        this.responseCallbacks[message.MessageId] = callback;
      }
      this.webSocket.send(JSON.stringify(message));
    }
    else {
      this.messageQueue.push({
        message: message,
        callback: callback,
      });
    }
  }
}

export enum ConnectionStatus {
  Connecting,
  NotAuthenticated,
  Authenticated,
  NoConnection,
}

export interface WebSocketMessage<T = unknown> {
  Name: string;
  Data: T;
  MessageId?: string;
}

interface WebSocketMessageQueueItem {
  message: WebSocketMessage;
  callback?: (response: WebSocketMessage | null) => void;
}

interface WebSocketMessageAuthenticationData {
  IsAuthenticated: boolean;
}