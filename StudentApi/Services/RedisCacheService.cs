using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StudentApi.Constants;
using StudentApi.Services.Interfaces;

namespace StudentApi.Services;

/// <summary>
/// Provides a cache service implementation using Redis via <see cref="IDistributedCache"/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RedisCacheService"/> class
/// </remarks>
/// <param name="cache">The distributed cache instance to use</param>
public class RedisCacheService(IDistributedCache cache) : ICacheService
{
    /// <summary>
    /// Retrieves a cached item associated with the specified key asynchronously
    /// </summary>
    /// <typeparam name="T">The type of the cached item</typeparam>
    /// <param name="key">The unique key identifying the cached item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the cached item if found; otherwise, <c>null</c>
    /// </returns>
    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(data)) return default;

        return JsonSerializer.Deserialize<T>(data);
    }

    /// <summary>
    /// Stores an item in the cache asynchronously with an optional expiration time
    /// </summary>
    /// <typeparam name="T">The type of the item to cache</typeparam>
    /// <param name="key">The unique key to associate with the cached item</param>
    /// <param name="item">The item to cache</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task SetAsync<T>(string key, T item)
    {
        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(TimeSpan.FromSeconds(ApiConstants.CachedTimeOutInSec));

        var data = JsonSerializer.Serialize(item);
        await cache.SetStringAsync(key, data, options);
    }

    /// <summary>
    /// Removes a cached item associated with the specified key asynchronously
    /// </summary>
    /// <param name="key">The unique key identifying the cached item to remove</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}