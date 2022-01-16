import { Component, OnDestroy, OnInit } from '@angular/core';
import { JournalWorkerService } from './journal-worker.service';
import { ConnectionStatus, WebsocketService } from './websocket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  public readonly ConnectionStatus = ConnectionStatus;

  public constructor(
    public readonly websocketService: WebsocketService,
    public readonly journalWorkerService: JournalWorkerService) {
  }

  public ngOnInit(): void {
      
  }

  public ngOnDestroy(): void {
      
  }
}

