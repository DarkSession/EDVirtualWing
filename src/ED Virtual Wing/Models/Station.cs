using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("Station")]
    public class Station
    {
        [JsonIgnore]
        [Key]
        [Column]
        public long MarketId { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(256)")]
        public string? NameAddon { get; set; }

        [Column]
        public long? StarSystemId { get; set; }

        [ForeignKey("StarSystemId")]
        public StarSystem? StarSystem { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal DistanceFromStarLS { get; set; }

        [Column]
        public StationType StationType { get; set; }
    }

    public enum StationType : short
    {
        Coriolis = 1,
        Outpost,
        Orbis,
        FleetCarrier,
        CraterOutpost,
        Ocellus,
        CraterPort,
        AsteroidBase,
        Bernal,
        MegaShip,
        SurfaceStation,
        OnFootSettlement,
    }
}
