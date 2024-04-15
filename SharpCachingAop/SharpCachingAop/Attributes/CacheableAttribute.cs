namespace CachingAop.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : Attribute
{
    public string CacheKey { get; }

    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);

    public bool ReturnDeepCopy { get; set; } = false;//todo utilize this

    public CacheableAttribute(string cacheKey)
    {
        CacheKey = cacheKey;  
    }
}
