import { Component, OnInit } from '@angular/core';
import { Commander, GameActivity, GameExtraFlags, GameMode, GameVersion, Ship, VehicleStatusFlags } from 'src/app/interfaces/commander';
import { StationType } from 'src/app/interfaces/station';

@Component({
  selector: 'app-cmdr-test',
  templateUrl: './cmdr-test.component.html',
  styleUrls: ['./cmdr-test.component.css']
})
export class CmdrTestComponent implements OnInit {
  public commander1: Commander = {
    Name: "Test1",
    GameActivity: GameActivity.Dead,
    ExtraFlags: GameExtraFlags.InCombat,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Open,
    GameModeGroupName: "GroupNameTest",
    VehicleStatusFlags:
      VehicleStatusFlags.FsdCharging |
      VehicleStatusFlags.FsdCooldown |
      VehicleStatusFlags.HasLatLong |
      VehicleStatusFlags.ShieldsUp |
      VehicleStatusFlags.BeingInterdicted,
    Ship: Ship.FederalCorvette,
    Location: {
      StarSystem: {
        Name: "System Name",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: {
        Name: "Station Name",
        NameAddon: null,
        StarSystem: {
          Name: "System Name",
          LocationX: 1,
          LocationY: 2,
          LocationZ: 3,
        },
        DistanceFromStarLS: 123.56,
        StationType: StationType.Coriolis,
      },
      SystemBody: {
        BodyId: 22,
        Name: "Body Name",
      },
      Name: "Location",
      Latitude: 123.456,
      Altitude: 2345.678,
      Longitude: 34123.112,
    },
    Target: {
      StarSystem: {
        Name: "Target System Name",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Body: {
        BodyId: 33,
        Name: "target Body Name",
      },
      Name: "Fsd Target",
      ShipTarget: Ship.ImperialCutter,
      ShipTargetName: "Target name",
    },
  };
  public commander2: Commander = {
    Name: "Test2",
    GameActivity: GameActivity.Hyperspace,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Group,
    GameModeGroupName: "My group name",
    VehicleStatusFlags:
      VehicleStatusFlags.FsdCharging |
      VehicleStatusFlags.FsdJump |
      VehicleStatusFlags.ShieldsUp,
    Ship: Ship.Adder,
    Location: {
      StarSystem: {
        Name: "Sol",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: null,
      SystemBody: null,
      Name: "Location",
      Latitude: 0,
      Altitude: 0,
      Longitude: 0,
    },
    Target: {
      StarSystem: {
        Name: "Achenar",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Body: null,
      Name: null,
      ShipTarget: null,
      ShipTargetName: null,
    },
  };
  public commander3: Commander = {
    Name: "Test3",
    GameActivity: GameActivity.Supercruise,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Solo,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.None,
    Ship: Ship.AspScout,
    Location: {
      StarSystem: {
        Name: "Alioth",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: null,
      SystemBody: null,
      Name: "ALC-956 Demeter-class Cropper",
      Latitude: 0,
      Altitude: 0,
      Longitude: 0,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: Ship.CobraMkIII,
      ShipTargetName: "System Defence Force",
    },
  };
  public commander4: Commander = {
    Name: "Test4",
    GameActivity: GameActivity.Docked,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.ShieldsUp | VehicleStatusFlags.Docked,
    Ship: Ship.Eagle,
    Location: {
      StarSystem: {
        Name: "Sol",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: {
        Name: "Abraham Lincoln",
        NameAddon: null,
        StarSystem: {
          Name: "Sol",
          LocationX: 1,
          LocationY: 2,
          LocationZ: 3,
        },
        DistanceFromStarLS: 504,
        StationType: StationType.Orbis,
      },
      SystemBody: null,
      Name: null,
      Latitude: 0,
      Altitude: 0,
      Longitude: 0,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: Ship.FederalAssaultShip,
      ShipTargetName: "Random Dude",
    },
  };
  public commander5: Commander = {
    Name: "Test5",
    GameActivity: GameActivity.OnFoot,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.HasLatLong,
    Ship: Ship.ImperialCutter,
    Location: {
      StarSystem: {
        Name: "Sol",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: {
        Name: "Indongo Bay",
        NameAddon: null,
        StarSystem: {
          Name: "Sol",
          LocationX: 1,
          LocationY: 2,
          LocationZ: 3,
        },
        DistanceFromStarLS: 9162,
        StationType: StationType.OnFootSettlement,
      },
      SystemBody: {
        Name: "Oberon",
        BodyId: 0,
      },
      Name: null,
      Latitude: 111,
      Altitude: 222,
      Longitude: 25,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: null,
      ShipTargetName: null,
    },
  };
  public commander6: Commander = {
    Name: "Test6",
    GameActivity: GameActivity.Supercruise,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.FsdCooldown | VehicleStatusFlags.ShieldsUp,
    Ship: Ship.Type9,
    Location: {
      StarSystem: {
        Name: "Sol",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: null,
      SystemBody: null,
      Name: null,
      Latitude: 111,
      Altitude: 222,
      Longitude: 25,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: "Columbus",
      ShipTarget: null,
      ShipTargetName: null,
    },
  };
  public commander7: Commander = {
    Name: "Test7",
    GameActivity: GameActivity.Docked,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Base,
    GameMode: GameMode.Solo,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.Docked | VehicleStatusFlags.ShieldsUp,
    Ship: Ship.Python,
    Location: {
      StarSystem: {
        Name: "Shinrarta Dezhra",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: {
        Name: "Jameson Memorial",
        NameAddon: null,
        StarSystem: {
          Name: "Shinrarta Dezhra",
          LocationX: 1,
          LocationY: 2,
          LocationZ: 3,
        },
        DistanceFromStarLS: 504,
        StationType: StationType.Orbis,
      },
      SystemBody: null,
      Name: null,
      Latitude: 111,
      Altitude: 222,
      Longitude: 25,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: Ship.AllianceChieftain,
      ShipTargetName: "Random Ganker #123",
    },
  };
  public commander8: Commander = {
    Name: "Test8",
    GameActivity: GameActivity.None,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Solo,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.FsdCharging,
    Ship: Ship.Anaconda,
    Location: {
      StarSystem: {
        Name: "Colonia",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: {
        Name: "A0B-C1D",
        NameAddon: "Some fleet carrier",
        StarSystem: {
          Name: "Colonia",
          LocationX: 1,
          LocationY: 2,
          LocationZ: 3,
        },
        DistanceFromStarLS: 1,
        StationType: StationType.FleetCarrier,
      },
      SystemBody: null,
      Name: null,
      Latitude: 0,
      Altitude: 0,
      Longitude: 0,
    },
    Target: {
      StarSystem: {
        Name: "Tir",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Body: null,
      Name: null,
      ShipTarget: null,
      ShipTargetName: null,
    },
  };
  public commander9: Commander = {
    Name: "Test9",
    GameActivity: GameActivity.Supercruise,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.BeingInterdicted | VehicleStatusFlags.ShieldsUp,
    Ship: Ship.FerDeLance,
    Location: {
      StarSystem: {
        Name: "Shinrarta Dezhra",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: null,
      SystemBody: null,
      Name: null,
      Latitude: 0,
      Altitude: 0,
      Longitude: 0,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: Ship.FerDeLance,
      ShipTargetName: "CMDR Test99",
    },
  };

  public constructor() { }

  public ngOnInit(): void {
  }

}
