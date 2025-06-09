namespace StudentApi.Services.Interfaces;

/// <summary>
/// Defines a contract for a cache service that supports asynchronous operations
/// to get, set, and remove cached items by key
/// </summary>
public interface ICacheService
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
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Stores an item in the cache asynchronously with an optional expiration time
    /// </summary>
    /// <typeparam name="T">The type of the item to cache</typeparam>
    /// <param name="key">The unique key to associate with the cached item</param>
    /// <param name="item">The item to cache</param>
    /// </param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task SetAsync<T>(string key, T item);

    /// <summary>
    /// Removes a cached item associated with the specified key asynchronously
    /// </summary>
    /// <param name="key">The unique key identifying the cached item to remove</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task RemoveAsync(string key);
}

