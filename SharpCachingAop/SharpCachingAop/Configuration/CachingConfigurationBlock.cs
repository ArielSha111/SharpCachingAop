namespace CachingAop.Configuration;

public class CachingConfigurationBlock
{
    public TimeSpan DeadLockTimeOut { get; set; }
    public Dictionary<string, CacheSettings> ItemsConfiguration { get; set; }
}

