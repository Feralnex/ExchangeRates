using DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;

namespace DataAccess.Deserializers
{
    public class MidRateDeserializer : JsonDeserializer<MidRate>
    {
        protected override MidRate Create(Type objectType, JObject jObject)
        {
            string currency = jObject[nameof(currency)].ToObject<string>();
            string code = jObject[nameof(code)].ToObject<string>();
            decimal mid = jObject[nameof(mid)].ToObject<decimal>();

            return new MidRate()
            {
                Currency = new Currency()
                {
                    Name = currency,
                    Code = code
                },
                Mid = mid
            };
        }
    }
}
