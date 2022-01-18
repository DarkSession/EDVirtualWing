import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { Commander, GameActivity, VehicleStatusFlags } from 'src/app/interfaces/commander';

@Component({
  selector: 'app-commander',
  templateUrl: './commander.component.html',
  styleUrls: ['./commander.component.css']
})
export class CommanderComponent implements OnInit, OnChanges {
  @Input() commander!: Commander | null;
  public readonly GameActivity = GameActivity;
  public shieldsUp: boolean = false;
  public hardpointsDeployed: boolean = false;
  public silentRunning: boolean = false;

  public constructor() { }

  public ngOnInit(): void {
  }

  public ngOnChanges(): void {
    if (this.commander) {
      this.shieldsUp = this.hasFlag(VehicleStatusFlags.ShieldsUp);
      this.hardpointsDeployed = this.hasFlag(VehicleStatusFlags.HardpointsDeployed);
      this.silentRunning = this.hasFlag(VehicleStatusFlags.SilentRunning);
    }
  }

  private hasFlag(flag: VehicleStatusFlags): boolean {
    return ((this.commander?.VehicleStatusFlags ?? 0) & flag) == flag;
  }
}
