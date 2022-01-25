import { Component } from '@angular/core';
import * as dayjs from 'dayjs';
import { Commander, GameActivity, GameExtraFlags, GameMode, GameVersion, Ship, VehicleStatusFlags } from 'src/app/interfaces/commander';
import { StationType } from 'src/app/interfaces/station';

@Component({
  selector: 'app-example',
  templateUrl: './example.component.html',
  styleUrls: ['./example.component.css']
})
export class ExampleComponent {
  public commander1: Commander = {
    CommanderId: "1",
    Name: "Test1",
    LastEventDate: dayjs.utc().toISOString(),
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
      VehicleStatusFlags.BeingInterdicted |
      VehicleStatusFlags.FsdMassLocked | 
      VehicleStatusFlags.OverHeating,
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
    ShipHullHealth: 0.3456,
    ShipName: "TestShip 1",
  };
  public commander2: Commander = {
    CommanderId: "2",
    Name: "Test2",
    LastEventDate: dayjs.utc().toISOString(),
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
    ShipHullHealth: 1,
    ShipName: "TestShip 2",
  };
  public commander3: Commander = {
    CommanderId: "3",
    Name: "Test3",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.Supercruise,
    ExtraFlags: GameExtraFlags.InCombat,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Solo,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.HardpointsDeployed | VehicleStatusFlags.InFighter | VehicleStatusFlags.IsInDanger | VehicleStatusFlags.OverHeating,
    Ship: Ship.FederalCorvette,
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
    ShipHullHealth: 0.91,
    ShipName: "TestShip 3",
  };
  public commander4: Commander = {
    CommanderId: "4",
    Name: "Test4",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.Docked,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.ShieldsUp | VehicleStatusFlags.Docked | VehicleStatusFlags.InWing | VehicleStatusFlags.FsdMassLocked,
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
    ShipHullHealth: 1,
    ShipName: "TestShip 4",
  };
  public commander5: Commander = {
    CommanderId: "5",
    Name: "Test5",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.OnFoot,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.HasLatLong | VehicleStatusFlags.InWing,
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
    ShipHullHealth: 1,
    ShipName: "TestShip 5",
  };
  public commander6: Commander = {
    CommanderId: "6",
    Name: "Test6",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.Supercruise,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Odyssey,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.FsdCooldown | VehicleStatusFlags.ShieldsUp | VehicleStatusFlags.InWing,
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
    ShipHullHealth: 0.75,
    ShipName: "TestShip 6",
  };
  public commander7: Commander = {
    CommanderId: "7",
    Name: "Test7",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.Docked,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Base,
    GameMode: GameMode.Solo,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.Docked | VehicleStatusFlags.ShieldsUp | VehicleStatusFlags.FsdMassLocked,
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
    ShipHullHealth: 0.123,
    ShipName: "TestShip 7",
  };
  public commander8: Commander = {
    CommanderId: "8",
    Name: "Test8",
    LastEventDate: dayjs.utc().toISOString(),

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
    ShipHullHealth: 0.87,
    ShipName: "TestShip 8",
  };
  public commander9: Commander = {
    CommanderId: "9",
    Name: "Test9",
    LastEventDate: dayjs.utc().toISOString(),
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
    ShipHullHealth: 0.4444,
    ShipName: "TestShip 9",
  };
  public commander10: Commander = {
    CommanderId: "10",
    Name: "Test10",
    LastEventDate: dayjs.utc().toISOString(),
    GameActivity: GameActivity.InSrv,
    ExtraFlags: GameExtraFlags.None,
    GameVersion: GameVersion.Horizons,
    GameMode: GameMode.Open,
    GameModeGroupName: "",
    VehicleStatusFlags: VehicleStatusFlags.ShieldsUp | VehicleStatusFlags.HasLatLong,
    Ship: Ship.Anaconda,
    Location: {
      StarSystem: {
        Name: "Sol",
        LocationX: 1,
        LocationY: 2,
        LocationZ: 3,
      },
      Station: null,
      SystemBody: {
        Name: "Europa",
        BodyId: 0,
      },
      Name: null,
      Latitude: 422,
      Altitude: 224,
      Longitude: 3.4,
    },
    Target: {
      StarSystem: null,
      Body: null,
      Name: null,
      ShipTarget: null,
      ShipTargetName: null,
    },
    ShipHullHealth: 0.234,
    ShipName: "TestShip 10",
  };

  public constructor() { }

}