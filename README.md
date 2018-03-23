# Caching.Memory

A adaptor for an in memory implementation for the core caching framework

### What does it do?

The provides an in memory cache facility. Please note, this cache is local to the instance your application is running on, i.e. it is not distributed.

### Why use it?
Its fast. Compared to a distributed cache (redis), its going to be ~10k times faster to fetch from. However, this performance comes with some drawbacks.

You need to ensure the data you store in this cache is fit for purpose. As the local cache is not distributed, and is local to your instance only, you can _not_ share state with it.
Expiration of entries is non deterministic. It is quite possible (and you should assume its the norm), that instance #1 contains an entry, and instance #2 does not. 
Obviously, it uses memory on your local instance, this needs to be considered, as your environment may not have much memory available.

In short:
* Adding *transactional* data to this cache is not a good idea (use [Beatles.Caching.Redis](https://github.com/eShopWorld/beatles-caching-redis) instead)
* Adding *static* data, like reference data is a good idea.

#### Further reading
* Please refer to [Beatles.Caching](https://github.com/eShopWorld/beatles-caching) for framework details
* Please refer to Microsoft's [Memory​Cache](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache) page for specifics of the cache
* App/Web.config cache configuration section [system.runtime.caching](https://github.com/dotnet/docs/blob/master/docs/framework/configure-apps/file-schema/runtime/system-runtime-caching-element-cache-settings.md)


### IoC container registration
```c#
public class MemoryCacheModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MemoryCacheFactory>().SingleInstance();

        // default resolution       ICache -> MemoryCache
        builder.RegisterSource(new CacheRegistrationSource<MemoryCacheFactory>(typeof(ICache<>)));

        // optional, only required if you use the specific interface
        // for local cache          ILocalCache -> MemoryCache
        builder.RegisterSource(new CacheRegistrationSource<MemoryCacheFactory>(typeof(ILocalCache<>)));
    }
}
```
