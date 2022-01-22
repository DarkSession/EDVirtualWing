import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConnectionStatus, WebsocketService } from './websocket.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

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

  public constructor(
    public readonly websocketService: WebsocketService) {
    websocketService.onConnectionStatusChanged.pipe(untilDestroyed(this)).subscribe((connectionStatus: ConnectionStatus) => {
      this.isLoggedIn = (connectionStatus == ConnectionStatus.Authenticated);
    });
  }

  public ngOnInit(): void {
  }

  public ngOnDestroy(): void {
  }

  public toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }
}

