namespace CachingAop.Caching;

public interface ICachingProvider
{
    public CacheItem<T> Get<T>(string cacheKey);
    public Task<CacheItem<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);
    public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration);
    public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default);
}

