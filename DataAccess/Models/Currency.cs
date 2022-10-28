using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Currency
    {
        [Key]
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
