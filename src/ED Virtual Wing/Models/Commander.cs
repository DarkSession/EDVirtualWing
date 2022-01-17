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
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
    }

    public enum GameActivity
    {
        None = 0,
        Supercruise,
        Hyperspace,
        Combat,
    }
}
