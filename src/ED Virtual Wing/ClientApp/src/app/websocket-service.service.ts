import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WebsocketServiceService {
  public connectionStatus: ConnectionStatus = ConnectionStatus.Connecting;

  public constructor() {
    this.initalize();
  }

  private initalize(): void {
    const webSocketUrl: string = ((window.location.protocol == "http:") ? "ws://" : "wss://") + window.location.hostname + ":" + environment.webSocketPort + "/ws";
    this.connectionStatus = ConnectionStatus.NotAuthenticated;
    const webSocket = new WebSocket(webSocketUrl);
    webSocket.onopen = () => {
      if (!environment.production) {
        console.log("WebSocket.onopen");
      }
    };
    webSocket.onclose = (event: CloseEvent) => {
      if (!environment.production) {
        console.log("WebSocket.onclose", event);
      }
      if (!event.wasClean) {
        setTimeout(() => {
          this.initalize();
        }, 10000);
        this.connectionStatus = ConnectionStatus.NoConnection;
        if (!environment.production) {
          console.log("Unclean close. Scheduling another connection in 10s.");
        }
      }
    };
    webSocket.onerror = (event: Event) => {
      if (!environment.production) {
        console.log("WebSocket.onerror", event);
      }
    };
    webSocket.onmessage = (event: MessageEvent) => {
      if (!environment.production) {
        console.log("WebSocket.onmessage", event);
      }
      const message: WebSocketMessage = JSON.parse(event.data);
      this.processMessage(message);
    };
  }

  private async processMessage(message: WebSocketMessage): Promise<void> {
    switch (message.Name) {
      case "Authentication": {
        const authenticationData: WebSocketMessageAuthenticationData = message.Data;
        if (authenticationData.IsAuthenticated) {
          this.connectionStatus = ConnectionStatus.Authenticated;
        }
        else {
          this.connectionStatus = ConnectionStatus.NotAuthenticated
        }
        break;
      }
    }
  }
}

export enum ConnectionStatus {
  Connecting,
  NotAuthenticated,
  Authenticated,
  NoConnection,
}

export interface WebSocketMessage {
  Name: string;
  Data: any;
}

interface WebSocketMessageAuthenticationData {
  IsAuthenticated: boolean;
}