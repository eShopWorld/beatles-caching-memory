using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Eshopworld.Caching.Core;

namespace Eshopworld.Caching.Memory
{
    /// <summary>
    /// implements <see cref="MemoryCache"/> as a cache. Items added to the cache will expire after the specified duration in <see cref="CacheItem{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of object to store</typeparam>
    public class MemoryCache<T> : ILocalCache<T>
    {
        private readonly ObjectCache cache;

        public MemoryCache(): this(System.Runtime.Caching.MemoryCache.Default) {}

        public MemoryCache(ObjectCache cache)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public T Add(CacheItem<T> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            cache.Add(item.Key,item.Value,DateTimeOffset.UtcNow.Add(item.Duration));
            return item.Value;
        }

        public Task<T> AddAsync(CacheItem<T> item) => Task.FromResult(Add(item));

        public void Set(CacheItem<T> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            cache.Set(item.Key, item.Value, DateTimeOffset.UtcNow.Add(item.Duration));
        }

        public Task SetAsync(CacheItem<T> item)
        {
            Set(item);
            return Task.CompletedTask;
        }

        public bool Exists(string key) => cache.Contains(key);
        public Task<bool> ExistsAsync(string key) => Task.FromResult(Exists(key));


        public void Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            
            cache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }




        public T Get(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return (T)cache.Get(key);
        }

        public Task<T> GetAsync(string key) => Task.FromResult(Get(key));


        public CacheResult<T> GetResult(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return cache.Contains(key) ? new CacheResult<T>(true,Get(key)) : CacheResult<T>.Miss();
        }

        public Task<CacheResult<T>> GetResultAsync(string key) => Task.FromResult(GetResult(key));

        public IEnumerable<KeyValuePair<string, T>> Get(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            return keys.Select(k => new KeyValuePair<string,T>(k,Get(k)));
        }

        public Task<IEnumerable<KeyValuePair<string, T>>> GetAsync(IEnumerable<string> keys) => Task.FromResult(Get(keys));
    }
}