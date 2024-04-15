using Newtonsoft.Json;

namespace CachingAop.Serialization;

public class DefaultSerializationProvider : ISerializationProvider
{
    public string SerializeObject(object? value)
    {
        return JsonConvert.SerializeObject(value);
    }
}

