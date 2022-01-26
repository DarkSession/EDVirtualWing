import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-fdev-auth',
  templateUrl: './fdev-auth.component.html',
  styleUrls: ['./fdev-auth.component.css']
})
export class FdevAuthComponent implements OnInit {
  public errors: string[] = [];

  public constructor(
    private readonly httpClient: HttpClient,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly appService: AppService,
    private readonly webSocketService: WebsocketService
  ) { }

  public ngOnInit(): void {
    const code = this.route.snapshot.queryParams["code"];
    const state = this.route.snapshot.queryParams["state"];
    if (code && state) {
      this.authenticate(code, state);
    }
  }

  private async authenticate(code: string, state: string): Promise<void> {
    if (this.appService.isLoading) {
      return;
    }
    this.appService.setLoading(true);
    const fdevAuthResponseUrl: string = window.location.protocol + "//" + window.location.hostname + ":" + environment.backendPort + "/user/fdev-auth";
    const fdevAuthResponse = await this.httpClient.post<FDevAuthResponse>(fdevAuthResponseUrl, {
      State: state,
      Code: code,
    }, {
      withCredentials: true,
    }).toPromise();
    if (fdevAuthResponse.Success) {
      this.webSocketService.reconnect();
      this.router.navigate(["/"]);
    }
    else {
      this.errors = fdevAuthResponse.Error ?? [];
    }
    this.appService.setLoading(false);
  }
}

interface FDevAuthResponse {
  Success: boolean;
  Error?: string[];
}