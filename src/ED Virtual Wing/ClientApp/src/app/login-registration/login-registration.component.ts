import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login-registration',
  templateUrl: './login-registration.component.html',
  styleUrls: ['./login-registration.component.css']
})
export class LoginRegistrationComponent implements OnInit {
  public formMode: FormMode = FormMode.Login;
  public loading: boolean = false;
  public readonly FormMode = FormMode;

  public constructor() { }

  public ngOnInit(): void {
  }

  public toggleMode(): void {
    this.formMode = (this.formMode == FormMode.Registration) ? FormMode.Login : FormMode.Registration;
  }

  public register(): void {
    if (this.loading) {
      return;
    }
    this.loading = true;
  }

  public login(): void {
    if (this.loading) {
      return;
    }
    this.loading = true;
  }
}

enum FormMode {
  Login,
  Registration,
}