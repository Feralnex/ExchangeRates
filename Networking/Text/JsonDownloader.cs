using Networking.Exceptions;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Networking.Text
{
    public class JsonDownloader : Downloader
    {
        protected override T Deserialize<T>(byte[] data, Encoding encoding)
        {
            string serializedValue = encoding.GetString(data);

            try
            {
                T value = JsonConvert.DeserializeObject<T>(serializedValue);

                return value;
            }
            catch (Exception exception)
            {
                throw new DeserializationException(exception.Message);
            }
        }
    }
}
