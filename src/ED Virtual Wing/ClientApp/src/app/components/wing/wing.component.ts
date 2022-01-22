import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Commander } from 'src/app/interfaces/commander';
import { Wing } from 'src/app/interfaces/wing';
import { WebSocketMessage, WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-wing',
  templateUrl: './wing.component.html',
  styleUrls: ['./wing.component.css']
})
export class WingComponent implements OnInit {
  public wing: Wing | null = null;
  public commanders: Commander[] = [];

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly route: ActivatedRoute
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
      if (response !== null) {
        this.wing = response.Data.Wing;
        this.commanders = response.Data.Commanders;
      }
    }
  }
}

interface WingSubscribeResponse {
  Wing: Wing;
  Commanders: Commander[];
}

interface CommanderUpdatedMessage {
  Commander: Commander;
  Wing: Wing;
}