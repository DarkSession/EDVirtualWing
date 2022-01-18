import { StarSystem } from "./star-system";
import { Station } from "./station";

export interface Commander {
    Name: string;
    GameActivity: GameActivity;
    CurrentStarSystem: StarSystem;
    CurrentStation: Station;
    GameVersion: GameVersion;
    GameMode: GameMode;
    GameModeGroupName: string;
    VehicleStatusFlags: VehicleStatusFlags;
}

export enum GameActivity {
    None = 0,
    Supercruise,
    Hyperspace,
    Combat,
    Docked,
    Landed,
}

export enum GameVersion {
    Base = 0,
    Horizons,
    Odyssey,
}

export enum GameMode {
    Unknown,
    Open,
    Group,
    Solo,
}

export enum VehicleStatusFlags {
    None = 0,
    Docked = 1,
    Landed = 2,
    LandingGearDeployed = 4,
    ShieldsUp = 8,
    Supercruise = 16,
    FlightAssistOff = 32,
    HardpointsDeployed = 64,
    InWing = 128,
    LightsOn = 256,
    CargoScoopDeployed = 512,
    SilentRunning = 1024,
    ScoopingFuel = 2048,
    SrvHandbrake = 4096,
    SrvUsingTurretView = 8192,
    /// <summary>
    /// SRV Turret Retracted (close to ship)
    /// </summary>
    SrvTurretRetracted = 16384,
    SrvDriveAssist = 32768,
    FsdMassLocked = 65536,
    FsdCharging = 131072,
    FsdCooldown = 262144,
    /// <summary>
    /// Low fuel, ( < 25% )
    /// </summary>
    LowFuel = 524288,
    /// <summary>
    /// Overheating ( > 100% )
    /// </summary>
    OverHeating = 1048576,
    HasLatLong = 2097152,
    IsInDanger = 4194304,
    BeingInterdicted = 8388608,
    InMainShip = 16777216,
    InFighter = 33554432,
    InSRV = 67108864,
    HudInAnalysisMode = 134217728,
    NightVision = 268435456,
    AltitudeFromAverageRadius = 536870912,
    FsdJump = 1073741824,
    SrvHighBeam = 2147483648,
}