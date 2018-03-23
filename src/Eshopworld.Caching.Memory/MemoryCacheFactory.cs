using Eshopworld.Caching.Core;

namespace Eshopworld.Caching.Memory
{
    public class MemoryCacheFactory : ICacheFactory
    {
        public ICache<T> CreateDefault<T>() => new MemoryCache<T>();

        public ICache<T> Create<T>(string name) => new MemoryCache<T>(
            new System.Runtime.Caching.MemoryCache(name));
    }
}