import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
  public loading = false;

  public constructor(
    private readonly websocketService: WebsocketService
  ) { }

  public ngOnInit(): void {
  }

  public async create(): Promise<void> {
    this.loading = true;
    const response = this.websocketService.sendMessageAndWaitForResponse<WingCreateResponse>("WingCreate", {
      Name: this.wingName.value,
    });
    if (response !== null) {
      // 
    }
  }
}

interface WingCreateResponse {

}