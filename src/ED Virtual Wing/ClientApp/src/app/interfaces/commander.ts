import { StarSystemBody } from "./star-system-body";
import { StarSystem } from "./star-system";
import { Station } from "./station";

export interface Commander {
    CommanderId: string;
    Name: string;
    GameActivity: GameActivity;
    ExtraFlags: GameExtraFlags;
    GameVersion: GameVersion;
    GameMode: GameMode;
    GameModeGroupName: string;
    VehicleStatusFlags: VehicleStatusFlags;
    Ship: Ship | null;
    Location: CommanderLocation;
    Target: CommanderTarget;
    ShipHullHealth: number;
    IsStreaming: boolean;
}

export interface CommanderTarget {
    StarSystem: StarSystem | null;
    Body: StarSystemBody | null;
    Name: string | null;
    ShipTarget: Ship | null;
    ShipTargetName: string | null;
}

export interface CommanderLocation {
    StarSystem: StarSystem | null;
    Station: Station | null;
    SystemBody: StarSystemBody | null;
    Name: string | null;
    Latitude: number;
    Altitude: number;
    Longitude: number;
}

export enum GameActivity {
    None = 0,
    Dead,
    Supercruise,
    Hyperspace,
    Docked,
    Landed,
    InSrv,
    OnFoot,
}

export enum GameExtraFlags {
    None = 0,
    InCombat = 1,
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

export enum OnFootStatusFlags {
    None = 0,
    OnFoot = 1,
    /// <summary>
    /// In taxi (or dropship/shuttle)
    /// </summary>
    InTaxi = 2,
    /// <summary>
    /// In multicrew (ie in someone else's ship)
    /// </summary>
    InMulticrew = 4,
    OnFootInStation = 8,
    OnFootOnPlanet = 16,
    AimDownSight = 32,
    LowOxygen = 64,
    LowHealth = 128,
    Cold = 256,
    Hot = 512,
    VeryCold = 1024,
    VeryHot = 2048,
    GlideMode = 4096,
    OnFootInHangar = 8192,
    OnFootSocialSpace = 16384,
    OnFootExterior = 32768,
    BreathableAtmosphere = 65536,
}

export enum Ship {
    SideWinder = 128049249,
    Eagle = 128049255,
    Hauler = 128049261,
    Adder = 128049267,
    ViperMkIII = 128049273,
    CobraMkIII = 128049279,
    Type6 = 128049285,
    Dolphin = 128049291,
    Type7 = 128049297,
    AspExplorer = 128049303,
    Vulture = 128049309,
    ImperialClipper = 128049315,
    FederalDropship = 128049321,
    Orca = 128049327,
    Type9 = 128049333,
    Python = 128049339,
    BelugaLiner = 128049345,
    FerDeLance = 128049351,
    Anaconda = 128049363,
    FederalCorvette = 128049369,
    ImperialCutter = 128049375,
    DiamondbackScout = 128671217,
    ImperialCourier = 128671223,
    DiamondbackExplorer = 128671831,
    ImperialEagle = 128672138,
    FederalAssaultShip = 128672145,
    FederalGunship = 128672152,
    ViperMkIV = 128672255,
    CobraMkIV = 128672262,
    Keelback = 128672269,
    AspScout = 128672276,
    Type10 = 128785619,
    KraitMkII = 128816567,
    AllianceChieftain = 128816574,
    AllianceCrusader = 128816581,
    AllianceChallenger = 128816588,
    KraitPhantom = 128839281,
    Mamba = 128915979,
}