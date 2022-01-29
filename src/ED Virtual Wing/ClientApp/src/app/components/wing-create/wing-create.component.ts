import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { Wing } from 'src/app/interfaces/wing';
import { WebsocketService } from 'src/app/websocket.service';

@Component({
  selector: 'app-wing-create',
  templateUrl: './wing-create.component.html',
  styleUrls: ['./wing-create.component.css']
})
export class WingCreateComponent implements OnInit {
  public wingName: FormControl = new FormControl("", [Validators.required, Validators.minLength(6), Validators.maxLength(64)]);
  public createWingForm = new FormGroup({
    wingName: this.wingName,
  });
  public errors: string[] = [];

  public constructor(
    private readonly websocketService: WebsocketService,
    private readonly router: Router,
    private readonly appService: AppService
  ) { }

  public ngOnInit(): void {
  }

  public async create(): Promise<void> {
    if (this.appService.isLoading || !this.createWingForm.valid) {
      return;
    }
    this.appService.setLoading(true);
    try {
      const response = await this.websocketService.sendMessageAndWaitForResponse<WingCreateResponse>("WingCreate", {
        Name: this.wingName.value,
      });
      if (response !== null) {
        if (response.Success) {
          this.router.navigate(["/team", response.Data.Wing.WingId]);
        }
        else {
          this.errors = response.Errors ?? [];
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

interface WingCreateResponse {
  Wing: Wing;
}