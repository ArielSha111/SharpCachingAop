using Newtonsoft.Json;

namespace CachingAop.Serialization;

/// <summary>
/// Default implementation of the <see cref="ISerializationProvider"/> interface using Newtonsoft.Json for object serialization.
/// </summary>
public class DefaultSerializationProvider : ISerializationProvider
{
    /// <summary>
    /// Serializes the specified object using Newtonsoft.Json.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A string representing the serialized object.</returns>
    public string SerializeObject(object? value)
    {
        return JsonConvert.SerializeObject(value);
    }
}