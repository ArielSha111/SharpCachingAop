namespace CachingAop.Serialization;

/// <summary>
/// Represents a provider for object serialization operations.
/// </summary>
public interface ISerializationProvider
{
    /// <summary>
    /// Serializes the specified object into a string representation.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A string representation of the serialized object.</returns>
    public string SerializeObject(object? value);
}