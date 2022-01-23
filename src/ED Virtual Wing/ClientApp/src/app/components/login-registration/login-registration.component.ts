import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { environment } from 'src/environments/environment';
import { WebsocketService } from '../../websocket.service';

@Component({
  selector: 'app-login-registration',
  templateUrl: './login-registration.component.html',
  styleUrls: ['./login-registration.component.css']
})
export class LoginRegistrationComponent implements OnInit {
  public formMode: FormMode = FormMode.Login;
  public readonly FormMode = FormMode;
  public userNameLogin: FormControl = new FormControl("", [Validators.required, Validators.minLength(4)]);
  public passwordLogin: FormControl = new FormControl("", [Validators.required, Validators.minLength(8)]);
  public userNameReg: FormControl = new FormControl("", [Validators.required, Validators.minLength(4)]);
  public passwordReg: FormControl = new FormControl("", [Validators.required, Validators.minLength(8)]);
  public emailAddress: FormControl = new FormControl("", [Validators.required, Validators.email]);
  public loginForm = new FormGroup({
    userName: this.userNameLogin,
    password: this.passwordLogin,
  });
  public registrationForm = new FormGroup({
    userName: this.userNameReg,
    password: this.passwordReg,
    emailAddress: this.emailAddress,
  });
  public errors: string[] = [];

  public constructor(
    private readonly httpClient: HttpClient,
    private readonly webSocketService: WebsocketService,
    private readonly router: Router,
    private readonly appService: AppService
  ) { }

  public ngOnInit(): void {
  }

  public toggleMode(): void {
    this.formMode = (this.formMode == FormMode.Registration) ? FormMode.Login : FormMode.Registration;
    this.errors = [];
  }

  public async register(): Promise<void> {
    if (this.appService.isLoading || !this.registrationForm.valid) {
      return;
    }
    this.appService.setLoading(true);
    try {
      const registrationUrl: string = window.location.protocol + "//" + window.location.hostname + ":" + environment.backendPort + "/user/register";
      const registrationData = {
        UserName: this.userNameReg.value,
        Email: this.emailAddress.value,
        Password: this.passwordReg.value,
      };
      const registrationResponse = await this.httpClient.post<RegistrationResponse>(registrationUrl, registrationData, {
        withCredentials: true,
      }).toPromise();
      if (!registrationResponse.Success) {
        this.errors = registrationResponse.Error;
      }
      else {
        this.loginSuccessful();
      }
    }
    catch (e) {
      console.error(e);
      this.errors = ["An error occured while communicating with the server"];
    }
    this.appService.setLoading(false);
  }

  public async login(): Promise<void> {
    if (this.appService.isLoading || !this.loginForm.valid) {
      return;
    }
    this.appService.setLoading(true);
    try {
      const loginUrl: string = window.location.protocol + "//" + window.location.hostname + ":" + environment.backendPort + "/user/login";
      const loginData = {
        UserName: this.userNameLogin.value,
        Password: this.passwordLogin.value,
      };
      const loginResponse = await this.httpClient.post<LoginResponse>(loginUrl, loginData, {
        withCredentials: true,
      }).toPromise();
      if (!loginResponse.Success) {
        this.errors = ["Username or password incorrect."];
      }
      else {
        this.loginSuccessful();
      }
    }
    catch (e) {
      console.error(e);
      this.errors = ["An error occured while communicating with the server"];
    }
    this.appService.setLoading(false);
  }

  private loginSuccessful(): void {
    this.webSocketService.reconnect();
    this.router.navigate(["/"]);
  }
}

enum FormMode {
  Login,
  Registration,
}

interface RegistrationResponse {
  Success: boolean;
  Error: string[];
}

interface LoginResponse {
  Success: boolean;
}