import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginResetPasswordComponent } from './login-reset-password.component';

describe('LoginResetPasswordComponent', () => {
  let component: LoginResetPasswordComponent;
  let fixture: ComponentFixture<LoginResetPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginResetPasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
