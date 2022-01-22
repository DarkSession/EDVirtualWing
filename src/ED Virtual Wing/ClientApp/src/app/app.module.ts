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
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CommanderComponent } from './components/commander/commander.component';
import { JournalWorkerComponent } from './components/journal-worker/journal-worker.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CmdrTestComponent } from './components/cmdr-test/cmdr-test.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginRegistrationComponent,
    CommanderComponent,
    JournalWorkerComponent,
    CmdrTestComponent,
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
    MatProgressBarModule,
    MatTooltipModule,
    RouterModule.forRoot(
      [
        {
          path: 'test',
          component: CmdrTestComponent,
        },
        {
          path: '',
          component: JournalWorkerComponent,
        }
      ]
    ),
    BrowserAnimationsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

