namespace CachingAop.Configuration;

/// <summary>
/// Represents a configuration block for caching settings, including deadlock timeout and item-specific cache settings.
/// </summary>
public class CachingConfigurationBlock
{
    /// <summary>
    /// Gets or sets the timeout duration for deadlock prevention during caching operations.
    /// </summary>
    /// <remarks>
    /// By default, no locking mechanism is applied (timeout set to zero seconds).
    /// </remarks>
    public TimeSpan DeadLockTimeOut { get; set; } = TimeSpan.FromSeconds(0);

    /// <summary>
    /// Gets or sets the configuration settings for individual cached items.
    /// </summary>
    public Dictionary<string, CacheSettings> ItemsConfiguration { get; set; }
}

