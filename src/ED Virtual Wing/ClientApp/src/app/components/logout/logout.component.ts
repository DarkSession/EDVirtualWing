import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  public constructor(
    private readonly httpClient: HttpClient,
    private readonly webSocketService: WebsocketService,
    private readonly router: Router,
    private readonly appService: AppService
  ) { }

  public ngOnInit(): void {
    this.logout();
  }

  private async logout(): Promise<void> {
    this.appService.setLoading(true);
    try {
      const logoutUrl: string = window.location.protocol + "//" + window.location.hostname + ":" + environment.backendPort + "/user/logout";
      const logoutResponse = await this.httpClient.get(logoutUrl, {
        withCredentials: true,
      }).toPromise();
      this.webSocketService.disconnect();
      this.router.navigate(["/login"]);
    }
    catch (e) {
      console.error(e);
    }
    finally {
      this.appService.setLoading(false);
    }
  }
}
