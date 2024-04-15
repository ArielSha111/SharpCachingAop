using CachingAop.Caching;
using CachingAop.Configuration;
using CachingAop.Serialization;
using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CachingAop.DI;

public static class CachingDiManager
{
    /// <summary>
    /// Extension method to register caching and aspect-oriented programming (AOP) services in the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">Optional. The <see cref="IConfiguration"/> containing caching configuration settings. If not provided, a default <see cref="CachingConfigurationBlock"/> will be registered.</param>
    /// <param name="useDefaultCachingProvider">Optional. Specifies whether to use the default caching provider. If set to <c>false</c>, an <see cref="ICachingProvider"/> should be registered in the service collection.</param>
    /// <param name="useDefaultSerializationProvider">Optional. Specifies whether to use the default serialization provider. If set to <c>false</c>, an <see cref="ISerializationProvider"/> should be registered in the service collection.</param>
    public static void SetSharpCachingAopRegistration(this IServiceCollection services,
        IConfiguration? configuration = null,
        bool useDefaultCachingProvider = true,
        bool useDefaultSerializationProvider = true)
    {
        if (useDefaultCachingProvider)
        {
            services.AddSingleton<ICachingProvider, MicrosoftCachingProvider>();
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

        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
    }

    public static void AddInterceptedSingleton<TInterface, TImplementation, TInterceptor>(
    this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IAsyncInterceptor
    {
        services.AddSingleton<TImplementation>();
        services.TryAddTransient<TInterceptor>();
        services.AddSingleton(provider =>
        {
            return GetProvider<TInterface, TImplementation, TInterceptor>(provider);
        }
       );
    }

    public static void AddInterceptedTransient<TInterface, TImplementation, TInterceptor>(
    this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IAsyncInterceptor
    {
        services.AddTransient<TImplementation>();
        services.TryAddTransient<TInterceptor>();
        services.AddTransient(provider =>
        {
            return GetProvider<TInterface, TImplementation, TInterceptor>(provider);
        }
       );
    }

    public static void AddInterceptedScoped<TInterface, TImplementation, TInterceptor>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IAsyncInterceptor
    {
        services.AddScoped<TImplementation>();
        services.TryAddTransient<TInterceptor>();
        services.AddScoped(provider =>
        {
            return GetProvider<TInterface, TImplementation, TInterceptor>(provider);
        }
       );
    }

    public static TInterface GetProvider<TInterface, TImplementation, TInterceptor>(IServiceProvider provider)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IAsyncInterceptor
    {
        var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
        var implementation = provider.GetRequiredService<TImplementation>();
        var interceptor = provider.GetRequiredService<TInterceptor>();
        return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(implementation, interceptor);
    }
}

