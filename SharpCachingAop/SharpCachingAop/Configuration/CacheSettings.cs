namespace CachingAop.Configuration;

public class CacheSettings
{
    public TimeSpan Duration { get; set; }
    public bool ReturnDeepCopy { get; set; }
}