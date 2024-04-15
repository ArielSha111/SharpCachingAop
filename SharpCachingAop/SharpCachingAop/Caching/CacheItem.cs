namespace CachingAop.Caching;

/// <summary>
/// Represents an item stored in the cache.
/// </summary>
/// <typeparam name="T">The type of the cached item.</typeparam>
public class CacheItem<T>
{
    /// <summary>
    /// Gets a value indicating whether the cache item has a valid value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Gets the value of the cache item.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheItem{T}"/> class with the specified value and whether it has a valid value.
    /// </summary>
    /// <param name="value">The value of the cache item.</param>
    /// <param name="hasValue">A value indicating whether the cache item has a valid value.</param>
    public CacheItem(T value, bool hasValue)
    {
        Value = value;
        HasValue = hasValue;
    }

    /// <summary>
    /// Returns a string representation of the cache item.
    /// </summary>
    /// <returns>A string representation of the cache item's value, or "<null>" if the value is null.</returns>
    public override string ToString()
    {
        return Value?.ToString() ?? "<null>";
    }
}
