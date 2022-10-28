using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public abstract class Rate
    {
        public int Id { get; set; }
        [Required]
        public Currency Currency { get; set; }
    }
}
