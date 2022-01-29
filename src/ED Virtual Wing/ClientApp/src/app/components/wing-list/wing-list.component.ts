import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { Wing } from 'src/app/interfaces/wing';
import { WebsocketService } from 'src/app/websocket.service';

@Component({
  selector: 'app-wing-list',
  templateUrl: './wing-list.component.html',
  styleUrls: ['./wing-list.component.css']
})
export class WingListComponent implements OnInit, OnDestroy {
  public wings: WingDetail[] = [];

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly appService: AppService,
    private readonly router: Router
  ) { }

  public ngOnInit(): void {
    this.requestWings();
    this.appService.addMenuItem({
      icon: "add",
      text: "Create team",
      callback: () => {
        this.create();
      },
    });
  }

  public ngOnDestroy(): void {
    this.appService.clearMenuItems();
  }

  public create(): void {
    this.router.navigate(["/team", "create"]);
  }

  private async requestWings(): Promise<void> {
    const response = await this.webSocketService.sendMessageAndWaitForResponse<WingsGetResponse>("WingsGet", {});
    if (response !== null && response.Success) {
      this.wings = response.Data.Wings;
    }
  }
}

interface WingsGetResponse {
  Wings: WingDetail[];
}

interface WingDetail extends Wing {
  MemberCount: number;
  MemberOnline: number;
}