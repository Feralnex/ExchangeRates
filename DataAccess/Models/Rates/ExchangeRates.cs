using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public abstract class ExchangeRates<T> where T : Rate
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Table { get; set; }
        [Required]
        [MaxLength(50)]
        public string No { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public List<T> Rates { get; set; }
    }
}
