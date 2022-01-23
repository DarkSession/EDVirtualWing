import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConnectionStatus, WebsocketService } from './websocket.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { LoadingComponent } from './components/loading/loading.component';
import { AppService } from './app.service';

@UntilDestroy()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  public readonly ConnectionStatus = ConnectionStatus;
  public isMenuOpen: boolean = false;
  public isLoggedIn: boolean = false;
  private loadingOverlayRef: OverlayRef | null = null;

  public constructor(
    public readonly websocketService: WebsocketService,
    private readonly overlay: Overlay,
    private readonly appService: AppService
  ) {
  }

  public ngOnInit(): void {
    this.websocketService.onConnectionStatusChanged.pipe(untilDestroyed(this)).subscribe((connectionStatus: ConnectionStatus) => {
      this.isLoggedIn = (connectionStatus == ConnectionStatus.Authenticated);
    });
    this.appService.loadingChanged.pipe(untilDestroyed(this)).subscribe((state: boolean) => {
      if (state) {
        if (this.loadingOverlayRef != null) {
          return;
        }
        this.loadingOverlayRef = this.overlay.create({
          positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
          backdropClass: "cdk-overlay-transparent-backdrop",
          hasBackdrop: true,
        });
        const loadingOverlay = new ComponentPortal(LoadingComponent);
        this.loadingOverlayRef.attach(loadingOverlay);
      }
      else {
        if (this.loadingOverlayRef == null) {
          return;
        }
        this.loadingOverlayRef.detach();
        this.loadingOverlayRef = null;
      }
    });
  }

  public ngOnDestroy(): void {
    this.loadingOverlayRef?.detach();
    this.loadingOverlayRef = null;
  }

  public toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }
}

