using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    public class Commander
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Column]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column]
        public DateTimeOffset JournalLastEventDate { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
