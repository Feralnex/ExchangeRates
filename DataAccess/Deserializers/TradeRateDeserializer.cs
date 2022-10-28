using DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;

namespace DataAccess.Deserializers
{
    public class TradeRateDeserializer : JsonDeserializer<TradeRate>
    {
        protected override TradeRate Create(Type objectType, JObject jObject)
        {
            string currency = jObject[nameof(currency)].ToObject<string>();
            string code = jObject[nameof(code)].ToObject<string>();
            decimal bid = jObject[nameof(bid)].ToObject<decimal>();
            decimal ask = jObject[nameof(ask)].ToObject<decimal>();

            return new TradeRate()
            {
                Currency = new Currency()
                {
                    Name = currency,
                    Code = code
                },
                Bid = bid,
                Ask = ask
            };
        }
    }
}
