﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Eshopworld.Caching.Core;
using Xunit;

namespace Eshopworld.Caching.Memory.Tests.Unit
{
    public class MemoryCacheTests
    {
        private const string CacheKey = "item";
        private MemoryCacheFactory cacheFactory;
        private MemoryCache<string> stringCache;

        public MemoryCacheTests()
        {

            cacheFactory = new MemoryCacheFactory();

            stringCache = (MemoryCache<string>) cacheFactory.CreateDefault<string>();
            stringCache.Remove(CacheKey);
        }


        [Fact]
        public void SetString_DoesNotThrow()
        {
            // Arrange
            // Act
            stringCache.Set(new CacheItem<string>(CacheKey, "Test", TimeSpan.FromSeconds(5)));
            // Assert
        }

        [Fact]
        public async Task SetAsyncString_DoesNotThrow()
        {
            // Arrange
            // Act
            await stringCache.SetAsync(new CacheItem<string>(CacheKey, "Test", TimeSpan.FromSeconds(5)));
            // Assert
        }


        [Fact]
        public void SetStringWithTimeout_ItemDoesNotExistAfterTimeout()
        {
            // Arrange
            // Act
            var duration = TimeSpan.FromSeconds(3);
            stringCache.Set(new CacheItem<string>(CacheKey, "Test", duration));

            // Assert
            Assert.True(stringCache.Exists(CacheKey));
            System.Threading.Thread.Sleep(duration.Add(TimeSpan.FromSeconds(1)));
            Assert.False(stringCache.Exists(CacheKey));
        }

        [Fact]
        public void GetString_ItemExistsInCache()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            var result = stringCache.Get(CacheKey);

            // Assert
            Assert.Equal(result, cacheValue);
        }

        [Fact]
        public async Task GetAsyncString_ItemExistsInCache()
        {
            // Arrange
            var cacheValue = "Test";

            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            var result = await stringCache.GetAsync(CacheKey);

            // Assert
            Assert.Equal(result, cacheValue);
        }




        [Fact]
        public void Remove_AfterRemove_GetReturnsNull()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            stringCache.Remove(CacheKey);

            // Assert
            var result = stringCache.Get(CacheKey);
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_AfterRemove_GetReturnsNull()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            await stringCache.RemoveAsync(CacheKey);

            // Assert
            var result = stringCache.Get(CacheKey);
            Assert.Null(result);
        }



        [Fact]
        public void Exists_AfterAdding_ReturnsTrue()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            // Assert
            Assert.True(stringCache.Exists(CacheKey));
        }

        [Fact]
        public async Task ExistsAsync_AfterAdding_ReturnsTrue()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            // Assert
            Assert.True(await stringCache.ExistsAsync(CacheKey));
        }

        [Fact]
        public void Exists_AfterAddingAndRemoving_ReturnsFalse()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));
            stringCache.Remove(CacheKey);
            // Act
            // Assert
            Assert.False(stringCache.Exists(CacheKey));
        }

        [Fact]
        public async Task Exists_AfterAddingAndRemoving_GetReturnsNull()
        {
            // Arrange
            var cacheValue = "Test";


            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(5)));

            // Act
            // Assert
            Assert.True(await stringCache.ExistsAsync(CacheKey));
        }


        [Fact]
        public void Expire_AfterSettingExpireAndWaiting_ItemDoesntExistInCache()
        {
            // Arrange
            var cacheValue = "Test";
            var expireIn = new TimeSpan(0, 0, 2);

            stringCache.Set(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(2)));

            // Act
            System.Threading.Thread.Sleep(expireIn.Add(TimeSpan.FromSeconds(5)));

            // Assert
            Assert.False(stringCache.Exists(CacheKey));
        }

        [Fact]
        public async Task ExpireAsync_AfterSettingExpireAndWaiting_ItemDoesntExistInCache()
        {
            // Arrange
            var cacheValue = "Test";
            var expireIn = new TimeSpan(0, 0, 2);

            await stringCache.SetAsync(new CacheItem<string>(CacheKey, cacheValue, TimeSpan.FromSeconds(2)));

            // Act
            await Task.Delay(expireIn.Add(TimeSpan.FromSeconds(5)));


            // Assert
            Assert.False(await stringCache.ExistsAsync(CacheKey));
        }


        [Fact]
        public void Get_SimpleObject_ReturnedObjectIsIdentical()
        {
            // Arrange
            var cache = cacheFactory.Create<SimpleObject>(nameof(SimpleObject));
            var value = SimpleObject.Create();
            cache.Set(new CacheItem<SimpleObject>(CacheKey, value, TimeSpan.FromSeconds(5)));

            // Act
            var result = cache.Get(CacheKey);

            // Assert
            Assert.Equal(result, value);
        }

        [Fact]
        public void Get_ComplexObject_ReturnedObjectIsIdentical()
        {
            // Arrange
            var cache = cacheFactory.Create<ComplexObject>(nameof(ComplexObject));

            var value = ComplexObject.Create();
            cache.Set(new CacheItem<ComplexObject>(CacheKey, value, TimeSpan.FromSeconds(5)));

            // Act
            var result = cache.Get(CacheKey);

            // Assert
            Assert.Equal(result, value);
        }


        [Fact]
        public void GetResult_SimpleObject_ReturnedObjectIsIdentical()
        {
            // Arrange
            var cache = cacheFactory.CreateDefault<SimpleObject>();
            var value = SimpleObject.Create();
            cache.Set(new CacheItem<SimpleObject>(CacheKey, value, TimeSpan.FromSeconds(5)));

            // Act
            var result = cache.GetResult(CacheKey);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasValue);
            Assert.Equal(result.Value, value);
        }

        [Fact]
        public async Task GetResultAsync_SimpleObject_ReturnedObjectIsIdentical()
        {
            // Arrange
            var cache = cacheFactory.CreateDefault<SimpleObject>();
            var value = SimpleObject.Create();
            cache.Set(new CacheItem<SimpleObject>(CacheKey, value, TimeSpan.FromSeconds(5)));

            // Act
            var result = await cache.GetResultAsync(CacheKey);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasValue);
            Assert.Equal(result.Value, value);
        }

        [Fact]
        public void GetResult_MissingObject_ResultDoesNotHaveValue()
        {
            // Arrange
            var cache = cacheFactory.CreateDefault<SimpleObject>();

            // Act
            var result = cache.GetResult("doesntExist");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasValue);
            Assert.Null(result.Value);
        }


        [Fact]
        public void Set_MultipeTasks_NoExceptions()
        {
            // Arrange
            const int numberOfItems = 100;
            var items = Enumerable.Range(0, numberOfItems).Select(i => SimpleObject.Create()).ToArray();
            var cache = cacheFactory.Create<SimpleObject>(nameof(SimpleObject));

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var loopResult = Parallel.For(0, 1000, i =>
            {
                var index = i % numberOfItems;
                var item = items[index];
                cache.Set(new CacheItem<SimpleObject>("item-" + index, item, TimeSpan.FromSeconds(5)));
            });
            stopwatch.Stop();

            // Assert
            Console.WriteLine("Duration:" + stopwatch.ElapsedMilliseconds);
            Assert.True(loopResult.IsCompleted);
        }

        [Fact]
        public void Get_MultipeTasks_NoExceptions()
        {
            // Arrange
            const int numberOfItems = 100;
            var cache = cacheFactory.Create<SimpleObject>(nameof(SimpleObject));

            Enumerable.Range(0, numberOfItems)
                .Select(i => Tuple.Create("item-" + i, SimpleObject.Create()))
                .All(_ => { cache.Set(new CacheItem<SimpleObject>(_.Item1, _.Item2, TimeSpan.FromSeconds(5))); return true; });

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var loopResult = Parallel.For(0, 1000, i =>
            {
                var index = i % numberOfItems;
                var result = cache.Get("item-" + index);

            });

            stopwatch.Stop();

            // Assert
            Console.WriteLine("Duration:" + stopwatch.ElapsedMilliseconds);
            Assert.True(loopResult.IsCompleted);
        }
    }
}