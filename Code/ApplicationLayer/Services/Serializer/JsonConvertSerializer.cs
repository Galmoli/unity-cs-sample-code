using Domain.Services.Serializer;
using Newtonsoft.Json;

namespace ApplicationLayer.Services.Serializer
{
    public class JsonConvertSerializer : ISerializerService
    {
        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}