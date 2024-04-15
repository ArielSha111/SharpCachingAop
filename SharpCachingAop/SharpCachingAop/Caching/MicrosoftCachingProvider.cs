using Microsoft.Extensions.Caching.Memory;

namespace CachingAop.Caching;

public class MicrosoftCachingProvider : ICachingProvider
{
    private readonly IMemoryCache _memoryCache;

    public MicrosoftCachingProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public CacheItem<T> Get<T>(string cacheKey)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T cacheValue))
        {
            return new CacheItem<T>(cacheValue, true);
        }
        else
        {
            return new CacheItem<T>(default, false);
        }
    }

    public async Task<CacheItem<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T cacheValue))
        {
            return new CacheItem<T>(cacheValue, true);
        }
        else
        {
            return new CacheItem<T>(default, false);
        }
    }

    public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        _memoryCache.Set(cacheKey, cacheValue, expiration);
    }

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        _memoryCache.Set(cacheKey, cacheValue, expiration);
        await Task.CompletedTask;
    }
}

