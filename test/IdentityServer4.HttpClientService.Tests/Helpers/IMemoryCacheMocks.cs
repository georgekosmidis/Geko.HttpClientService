using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.HttpClientService.Tests.Helpers
{
    public static class IMemoryCacheMocks
    {
        public static IMemoryCache Get(object cacheOutValue, bool tryGetReturns)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out cacheOutValue))
                .Returns(tryGetReturns);
            mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(new CacheEntryStub("",""));
                
            return mockMemoryCache.Object;
        }        
    }
}
