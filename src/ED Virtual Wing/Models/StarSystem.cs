using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("StarSystem")]
    public class StarSystem
    {
        [JsonIgnore]
        [Column]
        [Key]
        public long SystemAddress { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal LocationX { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal LocationY { get; set; }

        [Column(TypeName = "decimal(14,6)")]
        public decimal LocationZ { get; set; }

        [JsonIgnore]
        public IEnumerable<StarSystemBody>? Bodies { get; set; }
    }
}
