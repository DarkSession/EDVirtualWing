import { Component, Injector, OnInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Commander } from 'src/app/interfaces/commander';
import { Wing } from 'src/app/interfaces/wing';
import { WebSocketMessage, WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AppService } from 'src/app/app.service';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { WingInviteLinkComponent } from '../wing-invite-link/wing-invite-link.component';
import { InviteLinkData } from 'src/app/interfaces/invite-link-data';
import { OVERLAY_DATA } from 'src/app/injector/overlay-data';

@UntilDestroy()
@Component({
  selector: 'app-wing',
  templateUrl: './wing.component.html',
  styleUrls: ['./wing.component.css']
})
export class WingComponent implements OnInit {
  public wing: Wing | null = null;
  public commanders: Commander[] = [];
  public canManage: boolean = false;

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly appService: AppService,
    private readonly overlay: Overlay,
    private readonly injector: Injector,
    private readonly viewContainerRef: ViewContainerRef
  ) { }

  public ngOnInit(): void {
    this.subscribeToWing();
    this.webSocketService.on<CommanderUpdatedMessage>("CommanderUpdated").pipe(untilDestroyed(this)).subscribe((message: WebSocketMessage<CommanderUpdatedMessage>) => {
      if (!environment.production) {
        console.log("CommanderUpdated", message);
      }
      if (message.Data.Wing.WingId == this.wing?.WingId) {
        const commanderData = message.Data.Commander;
        const i = this.commanders.findIndex(c => c.CommanderId == commanderData.CommanderId);
        if (i === -1) {
          this.commanders.push(commanderData);
        }
        else {
          this.commanders[i] = commanderData;
        }
      }
    });
  }

  private async subscribeToWing(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", id);
    }
    if (id) {
      const response = await this.webSocketService.sendMessageAndWaitForResponse<WingSubscribeResponse>("WingSubscribe", {
        WingId: id,
      });
      if (response !== null && response.Success) {
        this.wing = response.Data.Wing;
        this.commanders = response.Data.Commanders;
        this.canManage = response.Data.CanManage;
        return;
      }
    }
    this.router.navigate(["/"]);
  }

  public async leaveDisband(): Promise<void> {

  }

  public async generateInvite(): Promise<void> {
    if (!this.wing || !this.canManage) {
      return;
    }
    this.appService.setLoading(true);
    const response = await this.webSocketService.sendMessageAndWaitForResponse<WingInviteResponse>("WingInvite", {
      WingId: this.wing.WingId,
    });
    if (response !== null) {
      if (response.Success) {
        const inviteLink = `${window.location.origin}/wing/join/${response.Data.Invite}`;
        const inviteData: InviteLinkData = {
          inviteLink: inviteLink,
        };
        const injectorData = Injector.create({
          parent: this.injector,
          providers: [
            { provide: OVERLAY_DATA, useValue: inviteData },
          ],
        });
        const overlayRef = this.overlay.create({
          positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
          backdropClass: "cdk-overlay-transparent-backdrop",
          hasBackdrop: true,
        });
        const loadingOverlay = new ComponentPortal(WingInviteLinkComponent, this.viewContainerRef, injectorData);
        overlayRef.attach(loadingOverlay);
        overlayRef.backdropClick().subscribe(() => overlayRef.dispose());
      }
    }
    this.appService.setLoading(false);
  }
}

interface WingSubscribeResponse {
  Wing: Wing;
  Commanders: Commander[];
  CanManage: boolean;
}

interface CommanderUpdatedMessage {
  Commander: Commander;
  Wing: Wing;
}

interface WingInviteResponse {
  Invite: string;
}