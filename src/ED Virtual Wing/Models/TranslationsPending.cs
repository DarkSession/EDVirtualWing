using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("TranslationsPending")]
    public class TranslationsPending
    {
        [Column]
        public int Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string NonLocalized { get; set; } = string.Empty;

        [Column(TypeName = "varchar(256)")]
        public string LocalizedExample { get; set; } = string.Empty;
    }
}
