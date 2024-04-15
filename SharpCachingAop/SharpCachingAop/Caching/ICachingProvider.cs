namespace CachingAop.Caching;

/// <summary>
/// Represents a provider for caching operations, allowing synchronous and asynchronous access to cached data.
/// </summary>
public interface ICachingProvider
{
    /// <summary>
    /// Retrieves the cached item associated with the specified key synchronously.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="cacheKey">The key identifying the cached item.</param>
    /// <returns>A <see cref="CacheItem{T}"/> containing the cached item, or a default value if the item is not found.</returns>
    public CacheItem<T> Get<T>(string cacheKey);

    /// <summary>
    /// Retrieves the cached item associated with the specified key asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="cacheKey">The key identifying the cached item.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation, containing a <see cref="CacheItem{T}"/> with the cached item, or a default value if the item is not found.</returns>
    public Task<CacheItem<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the specified item in the cache with the specified key and expiration time.
    /// </summary>
    /// <typeparam name="T">The type of the item to be cached.</typeparam>
    /// <param name="cacheKey">The key under which to cache the item.</param>
    /// <param name="cacheValue">The item to cache.</param>
    /// <param name="expiration">The expiration duration for the cached item.</param>
    public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Asynchronously sets the specified item in the cache with the specified key and expiration time.
    /// </summary>
    /// <typeparam name="T">The type of the item to be cached.</typeparam>
    /// <param name="cacheKey">The key under which to cache the item.</param>
    /// <param name="cacheValue">The item to cache.</param>
    /// <param name="expiration">The expiration duration for the cached item.</param>
    /// <param name="cancellationToken">Optional. The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default);
}