namespace CachingAop.Caching;

public class CacheItem<T>
{
    public bool HasValue { get; }
    public T Value { get; }
  
    public CacheItem(T value, bool hasValue)
    {
        Value = value;
        HasValue = hasValue;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? "<null>";
    }
}

