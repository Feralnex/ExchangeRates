using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class TradeExchangeRates : ExchangeRates<TradeRate>
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime TradingDate { get; set; }
    }
}
