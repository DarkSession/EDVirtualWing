import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Injector, OnDestroy, OnInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Commander } from 'src/app/interfaces/commander';
import { Wing } from 'src/app/interfaces/wing';
import { WebSocketMessage, WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AppService } from 'src/app/app.service';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { WingInviteLinkComponent } from '../wing-invite-link/wing-invite-link.component';
import { InviteLinkData } from 'src/app/interfaces/invite-link-data';
import { OVERLAY_DATA } from 'src/app/injector/overlay-data';
import * as dayjs from 'dayjs';
import { WingLeaveDisbandComponent } from '../wing-leave-disband/wing-leave-disband.component';
import { WingLeaveDisbandData } from 'src/app/interfaces/wing-leave-disband-data';

@UntilDestroy()
@Component({
  selector: 'app-wing',
  templateUrl: './wing.component.html',
  styleUrls: ['./wing.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WingComponent implements OnInit, OnDestroy {
  public wing: Wing | null = null;
  public commanders: Commander[] = [];
  public canManage: boolean = false;
  private leaveDisbandOverlayRef: OverlayRef | null = null;

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly appService: AppService,
    private readonly overlay: Overlay,
    private readonly injector: Injector,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly changeDetectorRef: ChangeDetectorRef
  ) { }

  public ngOnInit(): void {
    this.subscribeToWing();
    this.webSocketService.on<CommanderUpdatedMessage>("CommanderUpdated").pipe(untilDestroyed(this)).subscribe((message: WebSocketMessage<CommanderUpdatedMessage>) => {
      if (!environment.production) {
        console.log("CommanderUpdated", message);
      }
      if (message.Data.Wing.WingId === this.wing?.WingId) {
        const commanderData = message.Data.Commander;
        if (commanderData.LastEventDate) {
          commanderData.LastEventDate = dayjs(commanderData.LastEventDate);
        }
        commanderData.LastActivity = dayjs(commanderData.LastActivity);
        const i = this.commanders.findIndex(c => c.CommanderId === commanderData.CommanderId);
        if (i === -1) {
          this.commanders.push(commanderData);
        }
        else {
          this.commanders[i] = commanderData;
        }
        this.sortCommanders();
        this.changeDetectorRef.markForCheck();
      }
    });
    this.webSocketService.on<WingUnsubscribedData>("WingUnsubscribed").pipe(untilDestroyed(this)).subscribe((message: WebSocketMessage<WingUnsubscribedData>) => {
      if (message.Data.WingId === this.wing?.WingId) {
        this.router.navigate(["/"]);
      }
    });
  }

  public ngOnDestroy(): void {
    this.appService.clearMenuItems();
    this.hideLeaveDisband();
  }

  private async subscribeToWing(): Promise<void> {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.appService.clearMenuItems();
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
        for (const commanderData of this.commanders) {
          if (commanderData.LastEventDate) {
            commanderData.LastEventDate = dayjs.utc(commanderData.LastEventDate);
          }
          commanderData.LastActivity = dayjs.utc(commanderData.LastActivity);
        }
        this.canManage = response.Data.CanManage;
        this.sortCommanders();
        if (this.canManage) {
          this.appService.addMenuItem({
            icon: "person_add",
            text: "Create invite",
            callback: () => {
              this.generateInvite();
            },
          });
          this.appService.addMenuItem({
            icon: "manage_accounts",
            text: "Manage members",
            callback: () => {
              this.router.navigate(["/team", "admin", "members", id]);
            },
          });
          this.appService.addMenuItem({
            icon: "delete_forever",
            text: "Disband team",
            callback: () => {
              this.leaveDisband();
            },
          });
        }
        else {
          this.appService.addMenuItem({
            icon: "exit_to_app",
            text: "Leave team",
            callback: () => {
              this.leaveDisband();
            },
          });
        }
        this.changeDetectorRef.markForCheck();
        return;
      }
    }
    this.router.navigate(["/"]);
  }

  private sortCommanders(): void {
    const now = dayjs.utc();
    this.commanders = this.commanders.sort((a, b) => {
      let aIsOnline = false;
      if (typeof a.LastEventDate === 'object') {
        aIsOnline = (a.LastEventDate?.diff(now, "second") ?? 999) <= 120;
      }
      let bIsOnline = false;
      if (typeof b.LastEventDate === 'object') {
        bIsOnline = (b.LastEventDate?.diff(now, "second") ?? 999) <= 120;;
      }
      if (aIsOnline < bIsOnline) {
        return 1;
      }
      else if (aIsOnline > bIsOnline) {
        return -1;
      }
      else if (a.Name < b.Name) {
        return -1;
      }
      else if (a.Name > b.Name) {
        return 1;
      }
      return 0;
    });
  }


  private async leaveDisband(): Promise<void> {
    this.hideLeaveDisband();
    if (this.wing === null) {
      return;
    }
    this.leaveDisbandOverlayRef = this.overlay.create({
      positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
      backdropClass: "cdk-overlay-transparent-backdrop",
      hasBackdrop: true,
    });
    const data: WingLeaveDisbandData = {
      wing: this.wing,
      isAdmin: this.canManage,
    };
    const injectorData = Injector.create({
      parent: this.injector,
      providers: [
        { provide: OVERLAY_DATA, useValue: data },
        { provide: OverlayRef, useValue: this.leaveDisbandOverlayRef },
      ],
    });
    const loadingOverlay = new ComponentPortal(WingLeaveDisbandComponent, null, injectorData);
    this.leaveDisbandOverlayRef.attach(loadingOverlay);
    this.leaveDisbandOverlayRef.backdropClick().subscribe(() => {
      this.hideLeaveDisband();
    });
  }

  private hideLeaveDisband(): void {
    if (this.leaveDisbandOverlayRef !== null) {
      this.leaveDisbandOverlayRef.dispose();
      this.leaveDisbandOverlayRef = null;
    }
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
        const inviteLink = `${window.location.origin}/team/join/${response.Data.Invite}`;
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

  public trackByCommander(index: number, commander: Commander): string {
    return commander.CommanderId;
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

interface WingUnsubscribedData {
  WingId: string;
}