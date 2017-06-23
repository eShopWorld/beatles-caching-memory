namespace Beatles.Caching.Caches.MemoryCache
{
    public class MemoryCacheFactory : ICacheFactory
    {
        public ICache<T> CreateDefault<T>() => new MemoryCache<T>();

        public ICache<T> Create<T>(string name) => new MemoryCache<T>(new System.Runtime.Caching.MemoryCache(name));
    }
}