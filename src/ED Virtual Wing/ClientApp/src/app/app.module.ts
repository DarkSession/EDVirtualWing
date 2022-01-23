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
import { CmdrTestComponent } from './components/cmdr-test/cmdr-test.component';
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
import { FaqComponent } from './faq/faq.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginRegistrationComponent,
    CommanderComponent,
    JournalWorkerComponent,
    CmdrTestComponent,
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
    RouterModule.forRoot(
      [
        {
          path: 'test',
          component: CmdrTestComponent,
        },
        {
          path: 'wing',
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
              path: ':id',
              component: WingComponent,
            },
          ],
        },
        {
          path: 'about',
          component: AboutComponent,
        },
        {
          path: 'login',
          component: LoginRegistrationComponent,
          canActivate: [NotAuthenticatedGuard],
        },
        {
          path: '**',
          component: MainComponent,
          canActivate: [AuthenticationGuard],
        }
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

