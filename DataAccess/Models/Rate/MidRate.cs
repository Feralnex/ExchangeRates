using DataAccess.Deserializers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [JsonConverter(typeof(MidRateDeserializer))]
    public class MidRate : Rate
    {
        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##########}")]
        [Column(TypeName = "decimal(18,10)")]
        public decimal Mid { get; set; }
    }
}
