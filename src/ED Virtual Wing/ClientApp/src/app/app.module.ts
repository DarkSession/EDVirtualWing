import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { LoginRegistrationComponent } from './components/login-registration/login-registration.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommanderComponent } from './components/commander/commander.component';
import { JournalWorkerComponent } from './components/journal-worker/journal-worker.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ExampleComponent } from './components/example/example.component';
import { WingCreateComponent } from './components/wing-create/wing-create.component';
import { WingJoinComponent } from './components/wing-join/wing-join.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AboutComponent } from './components/about/about.component';
import { AuthenticationGuard } from './authentication.guard';
import { NotAuthenticatedGuard } from './not-authenticated.guard';
import { environment } from 'src/environments/environment';
import { WingListComponent } from './components/wing-list/wing-list.component';
import { WingComponent } from './components/wing/wing.component';
import { MatMenuModule } from '@angular/material/menu';
import { LoadingComponent } from './components/loading/loading.component';
import { WingInviteLinkComponent } from './components/wing-invite-link/wing-invite-link.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { JournalWorkerSetupHelpComponent } from './components/journal-worker-setup-help/journal-worker-setup-help.component';
import { MainComponent } from './components/main/main.component';
import { FaqComponent } from './components/faq/faq.component';
import { TosComponent } from './components/tos/tos.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatStepperModule } from '@angular/material/stepper';
import { FdevAuthComponent } from './components/fdev-auth/fdev-auth.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LogoutComponent } from './components/logout/logout.component';
import { WingLeaveDisbandComponent } from './components/wing-leave-disband/wing-leave-disband.component';
import { WingAdminMembersComponent } from './components/wing-admin-members/wing-admin-members.component';
import { H3Component } from './components/h3/h3.component';
import { H2Component } from './components/h2/h2.component';
import { LoginResetPasswordComponent } from './components/login-reset-password/login-reset-password.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginRegistrationComponent,
    CommanderComponent,
    JournalWorkerComponent,
    ExampleComponent,
    WingCreateComponent,
    WingJoinComponent,
    AboutComponent,
    WingListComponent,
    WingComponent,
    LoadingComponent,
    WingInviteLinkComponent,
    JournalWorkerSetupHelpComponent,
    MainComponent,
    FaqComponent,
    TosComponent,
    FdevAuthComponent,
    LogoutComponent,
    WingLeaveDisbandComponent,
    WingAdminMembersComponent,
    H3Component,
    H2Component,
    LoginResetPasswordComponent,
    UserProfileComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTooltipModule,
    MatSidenavModule,
    MatMenuModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatCheckboxModule,
    RouterModule.forRoot(
      [
        {
          path: 'example',
          component: ExampleComponent,
        },
        {
          path: 'faq',
          component: FaqComponent,
        },
        {
          path: 'tos',
          component: TosComponent,
        },
        {
          path: 'about',
          component: AboutComponent,
        },
        {
          path: 'auth',
          component: FdevAuthComponent,
          canActivate: [NotAuthenticatedGuard],
        },
        {
          path: 'login',
          component: LoginRegistrationComponent,
          canActivate: [NotAuthenticatedGuard],
        },
        {
          path: 'password-reset',
          component: LoginResetPasswordComponent,
          canActivate: [NotAuthenticatedGuard],
        },
        {
          path: 'logout',
          component: LogoutComponent,
          canActivate: [AuthenticationGuard],
        },
        {
          path: 'profile',
          component: UserProfileComponent,
          canActivate: [AuthenticationGuard],
        },
        {
          path: 'team',
          canActivateChild: [AuthenticationGuard],
          canActivate: [AuthenticationGuard],
          children: [
            {
              path: 'create',
              component: WingCreateComponent,
            },
            {
              path: 'list',
              component: WingListComponent,
            },
            {
              path: 'join/:invite',
              component: WingJoinComponent,
            },
            {
              path: 'admin/members/:id',
              component: WingAdminMembersComponent,
            },
            {
              path: ':id',
              component: WingComponent,
            },
          ],
        },
        {
          path: '**',
          component: MainComponent,
          canActivate: [AuthenticationGuard],
        },
      ],
      {
        enableTracing: !environment.production,
      },
    ),
    BrowserAnimationsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

