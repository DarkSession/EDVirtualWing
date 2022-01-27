using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ED_Virtual_Wing.Models
{
    [Table("Commander")]
    public class Commander
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

        [Column]
        public Guid CommanderId { get; set; } = Guid.NewGuid();

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
        public DateTimeOffset? LastEventDate { get; set; }

        [Column]
        public GameActivity GameActivity { get; set; }

        [Column]
        public GameExtraFlags ExtraFlags { get; set; }

        [Column]
        public GameVersion GameVersion { get; set; }

        [Column]
        public GameMode GameMode { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string? GameModeGroupName { get; set; }

        [Column]
        public VehicleStatusFlags VehicleStatusFlags { get; set; }

        [Column]
        public Ship? Ship { get; set; }

        /// <summary>
        /// Indicates if the Commander is currently streaming their game journal to the application.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public bool IsStreaming
        {
            get
            {
                return (LastEventDate is DateTimeOffset lastEventDate && (DateTimeOffset.Now - lastEventDate).TotalSeconds < 120);
            }
        }

        public CommanderLocation? Location { get; set; }

        public CommanderTarget? Target { get; set; }

        [Column(TypeName = "decimal(14,8)")]
        public decimal ShipHullHealth { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string? ShipName { get; set; }

        [Column]
        public Suit Suit { get; set; }
    }

    public class CommanderTarget
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonIgnore]
        [ForeignKey("CommanderId")]
        public Commander Commander { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonIgnore]
        [Column]
        public long? StarSystemId { get; set; }

        [ForeignKey("StarSystemId")]
        public StarSystem? StarSystem { get; set; }

        [ForeignKey("BodyId")]
        public StarSystemBody? Body { get; set; }

        [JsonIgnore]
        [Column]
        public int FallbackBodyId { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; } = string.Empty;

        [Column]
        public Ship? ShipTarget { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string ShipTargetName { get; set; } = string.Empty;

        public void ResetShipTarget()
        {
            ShipTarget = null;
            ShipTargetName = string.Empty;
        }
    }

    public class CommanderLocation
    {
        [JsonIgnore]
        [Column]
        public int Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonIgnore]
        [ForeignKey("CommanderId")]
        public Commander Commander { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonIgnore]
        [Column]
        public long? StarSystemId { get; set; }

        [ForeignKey("StarSystemId")]
        public StarSystem? StarSystem { get; set; }

        [JsonIgnore]
        [Column]
        public long? StationId { get; set; }

        [ForeignKey("StationId")]
        public Station? Station { get; set; }

        [JsonIgnore]
        [Column]
        public long? SystemBodyId { get; set; }

        [ForeignKey("SystemBodyId")]
        public StarSystemBody? SystemBody { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal Altitude { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal Longitude { get; set; }

        public void SetLocationSystem(StarSystem starSystem)
        {
            StarSystem = starSystem;
            SystemBody = null;
            Station = null;
            Name = null;
        }

        public void SetLocationBody(StarSystem starSystem, StarSystemBody starSystemBody)
        {
            StarSystem = starSystem;
            SystemBody = starSystemBody;
            Station = null;
            Name = null;
        }

        public void SetLocationBody(StarSystemBody? starSystemBody)
        {
            SystemBody = starSystemBody;
            Station = null;
            Name = null;
        }

        public void SetLocationStation(StarSystem starSystem, Station station)
        {
            StarSystem = starSystem;
            Station = station;
            Name = null;
        }

        public void SetLocationStation(Station? station)
        {
            Station = station;
            Name = null;
        }

        public void SetLocationName(string name)
        {
            Station = null;
            Name = name;
        }
    }

    public enum GameActivity
    {
        None = 0,
        Dead,
        Supercruise,
        Hyperspace,
        Docked,
        Landed,
        InSrv,
        OnFoot,
    }

    [Flags]
    public enum GameExtraFlags
    {
        None = 0,
        InCombat = 1,
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

    public enum Ship
    {
        SideWinder = 128049249,
        Eagle = 128049255,
        Hauler = 128049261,
        Adder = 128049267,
        [EnumMember(Value = "Viper")]
        ViperMkIII = 128049273,
        CobraMkIII = 128049279,
        Type6 = 128049285,
        Dolphin = 128049291,
        Type7 = 128049297,
        [EnumMember(Value = "Asp")]
        AspExplorer = 128049303,
        Vulture = 128049309,
        [EnumMember(Value = "Empire_Trader")]
        ImperialClipper = 128049315,
        [EnumMember(Value = "Federation_Dropship")]
        FederalDropship = 128049321,
        Orca = 128049327,
        Type9 = 128049333,
        Python = 128049339,
        BelugaLiner = 128049345,
        FerDeLance = 128049351,
        Anaconda = 128049363,
        [EnumMember(Value = "Federation_Corvette")]
        FederalCorvette = 128049369,
        [EnumMember(Value = "Cutter")]
        ImperialCutter = 128049375,
        [EnumMember(Value = "DiamondBack")]
        DiamondbackScout = 128671217,
        [EnumMember(Value = "Empire_Courier")]
        ImperialCourier = 128671223,
        [EnumMember(Value = "DiamondBackXL")]
        DiamondbackExplorer = 128671831,
        [EnumMember(Value = "Empire_Eagle")]
        ImperialEagle = 128672138,
        [EnumMember(Value = "Federation_Dropship_MkII")]
        FederalAssaultShip = 128672145,
        [EnumMember(Value = "Federation_Gunship")]
        FederalGunship = 128672152,
        [EnumMember(Value = "Viper_MkIV")]
        ViperMkIV = 128672255,
        CobraMkIV = 128672262,
        [EnumMember(Value = "Independant_Trader")]
        Keelback = 128672269,
        [EnumMember(Value = "Asp_Scout")]
        AspScout = 128672276,
        [EnumMember(Value = "Type9_Military")]
        Type10 = 128785619,
        [EnumMember(Value = "Krait_MkII")]
        KraitMkII = 128816567,
        [EnumMember(Value = "TypeX")]
        AllianceChieftain = 128816574,
        [EnumMember(Value = "TypeX_2")]
        AllianceCrusader = 128816581,
        [EnumMember(Value = "TypeX_3")]
        AllianceChallenger = 128816588,
        [EnumMember(Value = "Krait_Light")]
        KraitPhantom = 128839281,
        Mamba = 128915979,

        [EnumMember(Value = "independent_fighter")]
        IndepdenentFighter = 999999990,
        [EnumMember(Value = "gdn_hybrid_fighter_v1")]
        GuardianFighter = 999999991,
        [EnumMember(Value = "empire_fighter")]
        EmpireFighter = 999999992,
    }

    public enum Suit : short
    {
        Flight = 0,
        Maverick,
        Dominator,
        Artemis,
    }
}
