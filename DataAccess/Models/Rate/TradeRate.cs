using DataAccess.Deserializers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [JsonConverter(typeof(TradeRateDeserializer))]
    public class TradeRate : Rate
    {
        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##########}")]
        [Column(TypeName = "decimal(18,10)")]
        public decimal Bid { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:0.##########}")]
        [Column(TypeName = "decimal(18,10)")]
        public decimal Ask { get; set; }
    }
}
