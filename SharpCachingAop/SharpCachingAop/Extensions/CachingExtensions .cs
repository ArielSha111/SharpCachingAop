using CachingAop.Caching;
using CachingAop.Configuration;

namespace SharpCachingAop.Extensions;

public static class CachingProviderExtensions
{
    public static async Task<T> GetOrSetAsync<T>(this ICachingProvider cachingProvider, CachingConfigurationBlock _configurationBlock, string fullCacheKey)
    {
        throw new NotImplementedException();
    }

    public static T GetOrSet<T>(this ICachingProvider cachingProvider, CachingConfigurationBlock _configurationBlock, string fullCacheKey)
    {
        throw new NotImplementedException();
    }
}

