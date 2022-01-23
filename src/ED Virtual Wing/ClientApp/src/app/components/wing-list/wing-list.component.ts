import { Component, OnInit } from '@angular/core';
import { Wing } from 'src/app/interfaces/wing';
import { WebsocketService } from 'src/app/websocket.service';

@Component({
  selector: 'app-wing-list',
  templateUrl: './wing-list.component.html',
  styleUrls: ['./wing-list.component.css']
})
export class WingListComponent implements OnInit {
  public wings: Wing[] = [];

  public constructor(
    private readonly webSocketService: WebsocketService
  ) { }

  public ngOnInit(): void {
    this.requestWings();
  }

  private async requestWings(): Promise<void> {
    const response = await this.webSocketService.sendMessageAndWaitForResponse<WingsGetResponse>("WingsGet", {});
    if (response !== null && response.Success) {
      this.wings = response.Data.Wings;
    }
  }
}

interface WingsGetResponse {
  Wings: Wing[];
}