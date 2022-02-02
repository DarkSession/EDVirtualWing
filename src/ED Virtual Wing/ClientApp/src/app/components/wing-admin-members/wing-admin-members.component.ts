import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { WebsocketService } from 'src/app/websocket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-wing-admin-members',
  templateUrl: './wing-admin-members.component.html',
  styleUrls: ['./wing-admin-members.component.css']
})
export class WingAdminMembersComponent implements OnInit {
  public readonly WingMembershipStatus = WingMembershipStatus;
  public wingMembers: WingMemberData[] = [];
  public wingId: string = "";

  public constructor(
    private readonly webSocketService: WebsocketService,
    private readonly appService: AppService,
    private readonly route: ActivatedRoute
  ) { }

  public ngOnInit(): void {
    this.requestWingMemberList();
  }

  private async requestWingMemberList(): Promise<void> {
    if (this.appService.isLoading) {
      return;
    }
    this.wingId = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", this.wingId);
    }
    if (this.wingId) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingMembersGetResponseData>("WingMembersGet", {
          WingId: this.wingId,
        });
        if (response?.Success) {
          this.wingMembers = response.Data.WingMembers;
        }
      }
      catch (e) {
        console.error(e);
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }

  public async kickBan(member: WingMemberData): Promise<void> {
    if (this.appService.isLoading || !member.CanModify) {
      return;
    }
    this.wingId = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", this.wingId);
    }
    if (this.wingId) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingMembersGetResponseData>("WingMemberKick", {
          WingId: this.wingId,
          UserId: member.Id,
        });
        if (response?.Success) {
          member.Status = WingMembershipStatus.Banned;
        }
      }
      catch (e) {
        console.error(e);
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }

  public async approve(member: WingMemberData): Promise<void> {
    if (this.appService.isLoading || !member.CanModify) {
      return;
    }
    this.wingId = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", this.wingId);
    }
    if (this.wingId) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingMembersGetResponseData>("WingMemberApproveReject", {
          WingId: this.wingId,
          UserId: member.Id,
          Approve: true,
        });
        if (response?.Success) {
          member.Status = WingMembershipStatus.Joined;
        }
      }
      catch (e) {
        console.error(e);
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }

  public async reject(member: WingMemberData): Promise<void> {
    if (this.appService.isLoading || !member.CanModify) {
      return;
    }
    this.wingId = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", this.wingId);
    }
    if (this.wingId) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingMembersGetResponseData>("WingMemberApproveReject", {
          WingId: this.wingId,
          UserId: member.Id,
          Approve: false,
        });
        if (response?.Success) {
          member.Status = WingMembershipStatus.Banned;
        }
      }
      catch (e) {
        console.error(e);
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }

  public async unban(member: WingMemberData): Promise<void> {
    if (this.appService.isLoading || !member.CanModify) {
      return;
    }
    this.wingId = this.route.snapshot.paramMap.get('id')!;
    if (!environment.production) {
      console.log("Wing Id:", this.wingId);
    }
    if (this.wingId) {
      this.appService.setLoading(true);
      try {
        const response = await this.webSocketService.sendMessageAndWaitForResponse<WingMembersGetResponseData>("WingMemberUnban", {
          WingId: this.wingId,
          UserId: member.Id,
        });
        if (response?.Success) {
          member.Status = WingMembershipStatus.Left;
          this.wingMembers = this.wingMembers.filter(w => w != member);
        }
      }
      catch (e) {
        console.error(e);
      }
      finally {
        this.appService.setLoading(false);
      }
    }
  }
}

interface WingMembersGetResponseData {
  WingMembers: WingMemberData[];
}

interface WingMemberData {
  Id: string;
  Name: string;
  Joined: string;
  CanModify: boolean;
  Status: WingMembershipStatus;
}

enum WingMembershipStatus {
  Left = 0,
  PendingApproval,
  Joined,
  Banned,
}