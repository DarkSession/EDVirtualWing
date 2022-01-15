import { Component, OnDestroy, OnInit } from '@angular/core';
import { JournalWorkerService } from './journal-worker.service';
import { ConnectionStatus, WebsocketServiceService } from './websocket-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  public readonly ConnectionStatus = ConnectionStatus;

  public constructor(
    public readonly websocketServiceService: WebsocketServiceService,
    public readonly journalWorkerService: JournalWorkerService) {
  }

  public ngOnInit(): void {
      
  }

  public ngOnDestroy(): void {
      
  }
}

