using CachingAop.Attributes;
using CachingAop.Caching;
using CachingAop.Configuration;
using CachingAop.Serialization;
using Castle.DynamicProxy;
using System.Collections.Concurrent;

namespace CachingAop.Interceptors;
public class AsyncCachingInterceptor : IAsyncInterceptor
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> cacheLocks;

    private readonly ICachingProvider _cachingProvider;
    private readonly ISerializationProvider _serializationProvider;
    private readonly CachingConfigurationBlock _configurationBlock;


    public AsyncCachingInterceptor(ICachingProvider cachingProvider,
        ISerializationProvider serializationProvider,
        CachingConfigurationBlock configurationBlock)
    {
        cacheLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
        _cachingProvider = cachingProvider;
        _serializationProvider = serializationProvider;
        _configurationBlock = configurationBlock;
    }

    #region external
    public void InterceptSynchronous(IInvocation invocation)
    {
        if (ShouldIntercept(invocation))//TODO - add exception handling
        {
            HandleCacheableInvocation(invocation);
            return;
        }

        invocation.Proceed();
    }

    public async void InterceptAsynchronous(IInvocation invocation)
    {
        if (ShouldIntercept(invocation))//TODO - add exception handling
        {
            invocation.ReturnValue = await HandleAsyncCacheableInvocation<object>(invocation);
            return;
        }

        invocation.Proceed();
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        if (ShouldIntercept(invocation))//TODO - add exception handling
        {
            invocation.ReturnValue = HandleAsyncCacheableInvocation<TResult>(invocation);
            return;
        }

        invocation.Proceed();
    }
    #endregion

    #region internal
    private bool ShouldIntercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        return Attribute.IsDefined(method, typeof(CacheableAttribute));
    }

    private void HandleCacheableInvocation(IInvocation invocation)
    {
        var cacheAttribute = GetCacheAttribute(invocation);
        var cacheKey = GenerateCacheKey(cacheAttribute, invocation);
        var cacheValue = GetOrSet<object>(cacheKey, cacheAttribute, invocation);
        invocation.ReturnValue = cacheValue;
    }

    private async Task<TResult> HandleAsyncCacheableInvocation<TResult>(IInvocation invocation)
    {
        var cacheAttribute = GetCacheAttribute(invocation);
        var cacheKey = GenerateCacheKey(cacheAttribute, invocation);

        var cacheValue = await GetOrSetAsync<TResult>(cacheKey, cacheAttribute, invocation);
        return cacheValue;
    }

    private CacheableAttribute GetCacheAttribute(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        var cacheAttribute = (CacheableAttribute)Attribute.GetCustomAttribute(method, typeof(CacheableAttribute))!;
     
        cacheAttribute!.Duration =_configurationBlock.ItemsConfiguration[cacheAttribute.CacheKey].Duration;
        cacheAttribute.ReturnDeepCopy = _configurationBlock.ItemsConfiguration[cacheAttribute.CacheKey].ReturnDeepCopy;
     
        return cacheAttribute;
    }

    private object GetOrSet<T>(string cacheKey, CacheableAttribute cacheAttribute, IInvocation invocation)
    {
        var cacheValue = _cachingProvider.Get<object>(cacheKey);

        if (cacheValue.HasValue)
            return cacheValue.Value;

        var lockObj = cacheLocks.GetOrAdd(cacheKey, k => new SemaphoreSlim(1, 1));

        lockObj.Wait(_configurationBlock.DeadLockTimeOut);
        try
        {
            cacheValue = _cachingProvider.Get<object>(cacheKey);

            if (cacheValue.HasValue)
                return cacheValue.Value;

            invocation.Proceed();

            _cachingProvider.Set(cacheKey, invocation.ReturnValue, cacheAttribute.Duration);
            lockObj.Release();
            cacheLocks.TryRemove(cacheKey, out _);
        }
        catch
        {
            lockObj.Release();
            throw;
        }

        return invocation.ReturnValue;
    }

    private async Task<TResult> GetOrSetAsync<TResult>(string fullCacheKey,
        CacheableAttribute cacheAttribute, IInvocation invocation)
    {
        var captureProceedInfo = invocation.CaptureProceedInfo();

        var cacheValue = await _cachingProvider.GetAsync<TResult>(fullCacheKey);

        if (cacheValue.HasValue)
            return cacheValue.Value;

        var asyncLock = cacheLocks.GetOrAdd(fullCacheKey, _ => new SemaphoreSlim(1, 1));
        await asyncLock.WaitAsync(_configurationBlock.DeadLockTimeOut);
        try
        {
            cacheValue = await _cachingProvider.GetAsync<TResult>(fullCacheKey);

            if (cacheValue.HasValue)
                return cacheValue.Value;

            captureProceedInfo.Invoke();

            var result = await (Task<TResult>)invocation.ReturnValue;
            await _cachingProvider.SetAsync(fullCacheKey, result, cacheAttribute.Duration);
            asyncLock.Release();
            cacheLocks.TryRemove(fullCacheKey, out _);

            return result;
        }
        catch
        {
            asyncLock.Release();
            throw;
        }
    }

    private string GenerateCacheKey(CacheableAttribute cacheAttribute, IInvocation invocation)
    {
        var cacheKeyPrefix = $"{invocation.Method.Name}:{cacheAttribute.CacheKey}";

        var methodArguments = GetMethodArguments(invocation);
        var cacheKeyPostFix = string.Join(":", methodArguments.Values);

        var fullCacheKey = $"{cacheKeyPrefix}:{cacheKeyPostFix}";//TODO - consider add a namespace
        return fullCacheKey;
    }

    private Dictionary<string, string> GetMethodArguments(IInvocation invocation)
    {
        var methodArguments = invocation.Arguments
            .Select((arg, index) => new
            {
                ParameterName = invocation.Method.GetParameters()[index].Name,
                Value = arg
            })
            .ToDictionary(argInfo => argInfo.ParameterName, argInfo => FormatArgumentValue(argInfo.Value, argInfo.ParameterName));

        return methodArguments;
    }

    private string FormatArgumentValue(object argValue, string parameterName)
    {
        if (argValue is string || argValue.GetType().IsPrimitive)
            return $"[{parameterName},{argValue.GetType()},{argValue}]";

        return $"[{parameterName},{argValue.GetType()},{_serializationProvider.SerializeObject(argValue)}]";
    }
    #endregion
}
