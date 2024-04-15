using EasyCaching.Core;

namespace CachingAop.Caching;

public class EasyCahingProvider : ICachingProvider
{
    private readonly IEasyCachingProvider _cachingProvider;//todo replace with native in memory

    public EasyCahingProvider(IEasyCachingProvider cachingProvider)
    {
        _cachingProvider = cachingProvider;
    }

    public CacheItem<T> Get<T>(string cacheKey)
    {
        var cacheValue = _cachingProvider.Get<T>(cacheKey);
        var CacheItem = new CacheItem<T>(cacheValue.Value, cacheValue.HasValue);
        return CacheItem;
    }
    public async Task<CacheItem<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        var cacheValue = await _cachingProvider.GetAsync<T>(cacheKey, cancellationToken);
        var CacheItem = new CacheItem<T>(cacheValue.Value, cacheValue.HasValue);
        return CacheItem;
    }

    public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        _cachingProvider.Set(cacheKey, cacheValue, expiration);
    }

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        await _cachingProvider.SetAsync(cacheKey, cacheValue, expiration, cancellationToken);
    }
}

