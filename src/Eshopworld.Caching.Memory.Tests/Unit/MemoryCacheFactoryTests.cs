using System;
using Eshopworld.Caching.Core;
using Xunit;

namespace Eshopworld.Caching.Memory.Tests.Unit
{
    public class MemoryCacheFactoryTests
    {
        [Fact]
        public void CreateDefault_MultipleInstances_UsesCommonMemoryCache()
        {
            // Arrange
            var factory = new MemoryCacheFactory();
            var instance1 = factory.CreateDefault<string>();
            var instance2 = factory.CreateDefault<string>();

            // Act
            instance1.Add(new CacheItem<string>("test", "test-value", TimeSpan.FromMinutes(1)));
            var result = instance2.GetResult("test");
            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void Create_MultipleInstances_UsesDifferentMemoryCache()
        {
            // Arrange
            var factory = new MemoryCacheFactory();
            var instance1 = factory.Create<string>("instance1");
            var instance2 = factory.Create<string>("instance2");

            // Act
            instance1.Add(new CacheItem<string>("test", "test-value", TimeSpan.FromMinutes(1)));
            var result = instance2.GetResult("test");
            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasValue);
        }
    }
}
