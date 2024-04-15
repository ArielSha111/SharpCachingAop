namespace CachingAop.Attributes;

/// <summary>
/// Represents an attribute used to mark methods as cacheable.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : Attribute
{
    /// <summary>
    /// Gets the cache key associated with the method.
    /// </summary>
    public string CacheKey { get; }

    /// <summary>
    /// Gets or sets the duration for which the cached data is considered valid.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);

    /// <summary>
    /// Gets or sets a value indicating whether to return a deep copy of the cached data.
    /// </summary>
    /// <remarks>
    /// This property is currently unused but can be utilized for future enhancements.
    /// </remarks>
    public bool ReturnDeepCopy { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableAttribute"/> class with the specified cache key.
    /// </summary>
    /// <param name="cacheKey">The cache key associated with the method.</param>
    public CacheableAttribute(string cacheKey)
    {
        CacheKey = cacheKey;
    }
}