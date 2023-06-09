using System;
using System.Threading.Tasks;

namespace ForeverNote.Core.Caching
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public interface ICacheManager: IDisposable
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets or sets the value associated with the specified key asynchronosly
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<(T Result, bool FromCache)> TryGetValueAsync<T>(string key);

        /// <summary>
        /// Gets or sets the value associated with the specified key synchronosly
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        (T Result, bool FromCache) TryGetValue<T>(string key);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        Task SetAsync(string key, object data, int cacheTime);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool IsSet(string key);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        /// <param name="publisher">publisher</param>
        Task RemoveAsync(string key, bool publisher = true);

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="publisher">publisher</param>
        Task RemoveByPrefix(string prefix, bool publisher = true);

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="prefix">string prefix</param>
        /// <param name="publisher">publisher</param>
        Task RemoveByPrefixAsync(string prefix, bool publisher = true);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <param name="publisher">publisher</param>
        Task Clear(bool publisher = true);
        
    }
}
