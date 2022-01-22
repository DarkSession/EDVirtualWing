import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { Commander, GameActivity, GameExtraFlags, GameMode, GameVersion, Ship, VehicleStatusFlags } from 'src/app/interfaces/commander';
import { StationType } from 'src/app/interfaces/station';

@Component({
  selector: 'app-commander',
  templateUrl: './commander.component.html',
  styleUrls: ['./commander.component.css']
})
export class CommanderComponent implements OnInit, OnChanges {
  @Input() commander!: Commander | null;
  public readonly GameActivity = GameActivity;
  public readonly StationType = StationType;
  public shieldsUp: boolean = false;
  public fsdCharging: boolean = false;
  public fsdCooldown: boolean = false;
  public isBeingInterdicted: boolean = false;
  public inTeam: boolean = false;
  public inCombat: boolean = false;
  public inFighter: boolean = false;
  public weaponsDeployed: boolean = false;
  public showLatLong: boolean = false;
  public targetShip: string = "";
  public targetSystem: string = "";
  public gameMode: string = "";
  public gameVersion: string = "";
  public shipHealthSVG: number = 100;

  public constructor() { }

  public ngOnInit(): void {
  }

  public ngOnChanges(): void {
    if (this.commander) {
      this.shieldsUp = this.hasFlag(VehicleStatusFlags.ShieldsUp);
      this.fsdCharging = this.hasFlag(VehicleStatusFlags.FsdCharging);
      this.fsdCooldown = this.hasFlag(VehicleStatusFlags.FsdCooldown);
      this.isBeingInterdicted = this.hasFlag(VehicleStatusFlags.BeingInterdicted);
      this.showLatLong = this.hasFlag(VehicleStatusFlags.HasLatLong);
      this.inTeam = this.hasFlag(VehicleStatusFlags.InWing);
      this.inFighter = this.hasFlag(VehicleStatusFlags.InFighter);
      this.weaponsDeployed = this.hasFlag(VehicleStatusFlags.HardpointsDeployed);
      this.inCombat = this.hasExtraFlag(GameExtraFlags.InCombat);
      const shieldHullPercentage = (this.commander.ShipHullHealth * 100);
      if (shieldHullPercentage > 90) {
        this.shipHealthSVG = 100;
      }
      else if (shieldHullPercentage > 70) {
        this.shipHealthSVG = 80;
      }
      else if (shieldHullPercentage > 50) {
        this.shipHealthSVG = 60;
      }
      else if (shieldHullPercentage > 30) {
        this.shipHealthSVG = 40;
      }
      else if (shieldHullPercentage > 10) {
        this.shipHealthSVG = 20;
      }
      else {
        this.shipHealthSVG = 0;
      }
      let targetShip = "";
      if (this.commander.Target.ShipTargetName) {
        targetShip += this.commander.Target.ShipTargetName + " - ";
      }
      switch (this.commander.Target.ShipTarget) {
        case Ship.SideWinder: {
          targetShip += "Sidewinder";
          break;
        }
        case Ship.Eagle: {
          targetShip += "Eagle";
          break;
        }
        case Ship.Hauler: {
          targetShip += "Hauler";
          break;
        }
        case Ship.Adder: {
          targetShip += "Adder";
          break;
        }
        case Ship.ViperMkIII: {
          targetShip += "Viper MkIII";
          break;
        }
        case Ship.CobraMkIII: {
          targetShip += "Cobra MkIII";
          break;
        }
        case Ship.Type6: {
          targetShip += "Type-6 Transporter";
          break;
        }
        case Ship.Dolphin: {
          targetShip += "Dolphin";
          break;
        }
        case Ship.Type7: {
          targetShip += "Type-7 Transporter";
          break;
        }
        case Ship.AspExplorer: {
          targetShip += "Asp Explorer";
          break;
        }
        case Ship.Vulture: {
          targetShip += "Vulture";
          break;
        }
        case Ship.ImperialClipper: {
          targetShip += "Imperial Clipper";
          break;
        }
        case Ship.FederalDropship: {
          targetShip += "Federal Dropship";
          break;
        }
        case Ship.Orca: {
          targetShip += "Orca";
          break;
        }
        case Ship.Type9: {
          targetShip += "Type-9 Heavy";
          break;
        }
        case Ship.Python: {
          targetShip += "Python";
          break;
        }
        case Ship.BelugaLiner: {
          targetShip += "Beluga Liner";
          break;
        }
        case Ship.FerDeLance: {
          targetShip += "Fer-de-Lance";
          break;
        }
        case Ship.Anaconda: {
          targetShip += "Anaconda";
          break;
        }
        case Ship.FederalCorvette: {
          targetShip += "Federal Corvette";
          break;
        }
        case Ship.ImperialCutter: {
          targetShip += "Imperial Cutter";
          break;
        }
        case Ship.DiamondbackScout: {
          targetShip += "Diamondback Scout";
          break;
        }
        case Ship.ImperialCourier: {
          targetShip += "Imperial Courier";
          break;
        }
        case Ship.DiamondbackExplorer: {
          targetShip += "Diamondback Explorer";
          break;
        }
        case Ship.ImperialEagle: {
          targetShip += "Imperial Eagle";
          break;
        }
        case Ship.FederalAssaultShip: {
          targetShip += "Federal Assault Ship";
          break;
        }
        case Ship.FederalGunship: {
          targetShip += "Federal Gunship";
          break;
        }
        case Ship.ViperMkIV: {
          targetShip += "Viper MkIV";
          break;
        }
        case Ship.CobraMkIV: {
          targetShip += "Cobra MkIV";
          break;
        }
        case Ship.Keelback: {
          targetShip += "Keelback";
          break;
        }
        case Ship.AspScout: {
          targetShip += "Asp Scout";
          break;
        }
        case Ship.Type10: {
          targetShip += "Type-10 Defender";
          break;
        }
        case Ship.KraitMkII: {
          targetShip += "Krait MkII";
          break;
        }
        case Ship.AllianceChieftain: {
          targetShip += "Alliance Chieftain";
          break;
        }
        case Ship.AllianceCrusader: {
          targetShip += "Alliance Crusader";
          break;
        }
        case Ship.AllianceChallenger: {
          targetShip += "Alliance Challenger";
          break;
        }
        case Ship.KraitPhantom: {
          targetShip += "Krait Phantom";
          break;
        }
        case Ship.Mamba: {
          targetShip += "Mamba";
          break;
        }
      }
      this.targetShip = targetShip;
      let targetSystem: string = "";
      if (this.commander.Target.StarSystem && this.commander.Target.StarSystem.Name != this.commander.Location.StarSystem?.Name) {
        targetSystem = this.commander.Target.StarSystem.Name;
      }
      if (this.commander.Target.Name && this.commander.Target.Name != targetSystem) {
        if (targetSystem) {
          targetSystem += " - ";
        }
        targetSystem += this.commander.Target.Name;
      }
      this.targetSystem = targetSystem;
      switch (this.commander.GameMode) {
        case GameMode.Open: {
          this.gameMode = "Open";
          break;
        }
        case GameMode.Solo: {
          this.gameMode = "Solo";
          break;
        }
        case GameMode.Group: {
          this.gameMode = "Group: " + this.commander.GameModeGroupName;
          break;
        }
        default: {
          this.gameMode = "Unknown";
          break;
        }
      }
      switch (this.commander.GameVersion) {
        case GameVersion.Base: {
          this.gameVersion = "Base Game";
          break;
        }
        case GameVersion.Horizons: {
          this.gameVersion = "Horizons";
          break;
        }
        case GameVersion.Odyssey: {
          this.gameVersion = "Odyssey";
          break;
        }
        default: {
          this.gameVersion = "Unknown";
          break;
        }
      }
    }
  }

  private hasFlag(flag: VehicleStatusFlags): boolean {
    return ((this.commander?.VehicleStatusFlags ?? 0) & flag) == flag;
  }

  private hasExtraFlag(flag: GameExtraFlags): boolean {
    return ((this.commander?.ExtraFlags ?? 0) & flag) == flag;
  }
}
