import { OverlayRef } from '@angular/cdk/overlay';
import { Component, Inject, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { OVERLAY_DATA } from 'src/app/injector/overlay-data';
import { WingLeaveDisbandData } from 'src/app/interfaces/wing-leave-disband-data';
import { WebsocketService } from 'src/app/websocket.service';

@Component({
  selector: 'app-wing-leave-disband',
  templateUrl: './wing-leave-disband.component.html',
  styleUrls: ['./wing-leave-disband.component.css']
})
export class WingLeaveDisbandComponent implements OnInit {
  public confirmed: boolean = false;

  public constructor(
    @Inject(OVERLAY_DATA) public readonly data: WingLeaveDisbandData,
    private readonly overlayRef: OverlayRef,
    private readonly websocketService: WebsocketService,
    private readonly appService: AppService
  ) { }

  public ngOnInit(): void {
  }

  public async confim(): Promise<void> {
    if (!this.confirmed || this.appService.isLoading) {
      return;
    }
    try {
      this.appService.setLoading(true);
      await this.websocketService.sendMessageAndWaitForResponse("WingLeaveDisband", {
        WingId: this.data.wing.WingId,
      });
      this.cancel();
    }
    catch (e) {
      console.error(e);
    }
    finally {
      this.appService.setLoading(false);
    }
  }

  public cancel(): void {
    this.overlayRef.dispose();
  }
}
