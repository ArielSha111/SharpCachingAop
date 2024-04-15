namespace CachingAop.Serialization;

public interface ISerializationProvider
{
    public string SerializeObject(object? value);
}

