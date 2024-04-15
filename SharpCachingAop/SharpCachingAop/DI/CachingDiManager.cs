using CachingAop.Caching;
using CachingAop.Configuration;
using CachingAop.Interceptors;
using CachingAop.Serialization;
using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CachingAop.DI;

public static class CachingDiManager//add transient and scoped
{
    public static void AddSingletonWithCacheProvider<TInterface, TImplementation>(this IServiceCollection services,
        IConfiguration? configuration = null,
        bool useDefaultCachingProvider = true,
        bool useDefaultSerializationProvider = true)
    where TInterface : class
    where TImplementation : class, TInterface
    {
        if (useDefaultCachingProvider)//todo replace easycaching with native in memory to be the default
        {
            services.AddSingleton<ICachingProvider, MicrosoftCahingProvider>();
        }

        if(useDefaultSerializationProvider)
        {
            services.AddSingleton<ISerializationProvider, DefaultSerializationProvider>();
        }

        if (configuration is not null)
        {
            var cachingConfig = new CachingConfigurationBlock();
            configuration.GetSection(nameof(CachingConfigurationBlock)).Bind(cachingConfig);
            services.AddSingleton(cachingConfig);
        }

        services.AddInterceptedSingleton<TInterface, TImplementation, AsyncCachingInterceptor>();
    }

    private static void AddInterceptedSingleton<TInterface, TImplementation, Tinterceptor>(
    this IServiceCollection services)
    where TInterface : class
    where TImplementation : class, TInterface
    where Tinterceptor : class, IAsyncInterceptor
    {
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
        services.AddSingleton<TImplementation>();
        services.TryAddTransient<Tinterceptor>();
        services.AddSingleton(provider =>
        {
            return GetProvider<TInterface, TImplementation, Tinterceptor>(provider);
        }
       );
    }

    private static TInterface GetProvider<TInterface, TImplementation, Tinterceptor>(IServiceProvider provider)
       where TInterface : class
       where TImplementation : class, TInterface
       where Tinterceptor : class, IAsyncInterceptor
    {
        var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
        var impl = provider.GetRequiredService<TImplementation>();
        var interceptor = provider.GetRequiredService<Tinterceptor>();
        return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(impl, interceptor);
    }
}

