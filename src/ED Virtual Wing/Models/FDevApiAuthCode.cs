using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ED_Virtual_Wing.Models
{
    [Table("FDevApiAuthCode")]
    public class FDevApiAuthCode
    {
        [Key]
        [Column(TypeName = "varchar(128)")]
        public string State { get; set; } = string.Empty;
        [Column(TypeName = "varchar(128)")]
        public string Code { get; set; } = string.Empty;
        [Column]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    }
}
