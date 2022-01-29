import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { Wing } from 'src/app/interfaces/wing';
import { WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-wing-join',
  templateUrl: './wing-join.component.html',
  styleUrls: ['./wing-join.component.css']
})
export class WingJoinComponent implements OnInit {
  public errors: string[] = [];
  public wing: Wing | null = null;

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly appService: AppService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) { }

  public ngOnInit(): void {
    this.requestWingDetails(false);
  }

  private async requestWingDetails(join: boolean): Promise<void> {
    if (this.appService.isLoading) {
      return;
    }
    const invite = this.route.snapshot.paramMap.get("invite")!;
    if (!environment.production) {
      console.log("Wing invite:", invite);
    }
    if (invite) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingJoinResponse>("WingJoin", {
          Invite: invite,
          Join: join,
        });
        if (response !== null) {
          if (!response.Success) {
            this.errors = response.Errors ?? [];
          }
          else {
            this.wing = response.Data.Wing;
            if (response.Data.Joined) {
              this.router.navigate(["/team", this.wing.WingId]);
            }
          }
        }
      }
      catch (e) {
        console.error(e);
        this.errors = ["Communication with the server failed."];
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }

  public join(): Promise<void> {
    return this.requestWingDetails(true);
  }
}

export interface WingJoinResponse {
  Joined: boolean;
  Status: WingMembershipStatus;
  Wing: Wing;
}

enum WingMembershipStatus {
  Left = 0,
  PendingApproval,
  Joined,
  Banned,
}