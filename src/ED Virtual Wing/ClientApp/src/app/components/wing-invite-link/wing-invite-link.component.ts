import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OVERLAY_DATA } from 'src/app/injector/overlay-data';
import { InviteLinkData } from 'src/app/interfaces/invite-link-data';

@Component({
  selector: 'app-wing-invite-link',
  templateUrl: './wing-invite-link.component.html',
  styleUrls: ['./wing-invite-link.component.css']
})
export class WingInviteLinkComponent implements OnInit {

  public constructor(
    @Inject(OVERLAY_DATA) public readonly data: InviteLinkData,
    private readonly snackBar: MatSnackBar
  ) { }

  public ngOnInit(): void {
  }

  public async copy(): Promise<void> {
    await navigator.clipboard.writeText(this.data.inviteLink);
    this.snackBar.open("Copied to clipboard", "Dismiss", {
      duration: 2500,
    });
  }
}
