using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    public class Commander
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonIgnore]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        [Column]
        public DateTimeOffset JournalLastEventDate { get; set; }
        [Column]
        public GameActivity GameActivity { get; set; }
        [JsonIgnore]
        [Column]
        public long? CurrentStarSystemId { get; set; }
        [ForeignKey("CurrentStarSystemId")]
        public StarSystem? CurrentStarSystem { get; set; }
        [Column]
        public long? CurrentStationId { get; set; }
        [ForeignKey("CurrentStationId")]
        public Station? CurrentStation { get; set; }
        [Column]
        public GameVersion GameVersion { get; set; }
        [Column]
        public GameMode GameMode { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string? GameModeGroupName { get; set; }
        [Column]
        public VehicleStatusFlags VehicleStatusFlags { get; set; }
    }

    public enum GameActivity
    {
        None = 0,
        Supercruise,
        Hyperspace,
        Combat,
        Docked,
        Landed,
    }

    public enum GameVersion : short
    {
        Base = 0,
        Horizons,
        Odyssey,
    }

    public enum GameMode : short
    {
        Unknown,
        Open,
        Group,
        Solo,
    }

    [Flags]
    public enum VehicleStatusFlags : long
    {
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

    [Flags]
    public enum OnFootStatusFlags : long
    {
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
}
