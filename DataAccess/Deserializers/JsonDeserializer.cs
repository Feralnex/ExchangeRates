using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace DataAccess.Deserializers
{
    public abstract class JsonDeserializer<T> : JsonConverter
    {
        public override bool CanWrite => false;

        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType) => typeof(T).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
