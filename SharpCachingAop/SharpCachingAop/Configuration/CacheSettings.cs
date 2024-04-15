namespace CachingAop.Configuration;

/// <summary>
/// Represents settings for caching operations, including duration and whether to return a deep copy of cached data.
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Gets or sets the duration for which the cached data is considered valid.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to return a deep copy of the cached data.
    /// </summary>
    public bool ReturnDeepCopy { get; set; }
}
