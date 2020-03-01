using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using IdentityModel.Client;
using System.Net;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Models;
using System;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Infrastructure
{
    [TestClass]
    public class AccessTokenCacheManagerIntegrationTests
    {

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_EmptyCache_RetrieveNew()
        {
            var tokenResponse = await TokenResponseMock.GetValidResponseAsync("access_token", 3600);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse); }
             );

            Assert.AreEqual(tokenResponse, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_ReturneCached()
        {
            var tokenResponse1 = await TokenResponseMock.GetValidResponseAsync("access_token_1", 10);
            var tokenResponse2 = await TokenResponseMock.GetValidResponseAsync("access_token_2", 10);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            //should cache it and returne tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);

            //should not cache it and return tokenResponse1
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse2); }//now it is 2
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_RenewExpiredItem()
        {
            var tokenResponse1 = await TokenResponseMock.GetValidResponseAsync("access_token_1", 1);
            var tokenResponse2 = await TokenResponseMock.GetValidResponseAsync("access_token_2", 1);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            //should cache it and returne tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey1",
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);
            
            //sleep for cache expiration
            Thread.Sleep(1000);
            
            //should cache it and return tokenResponse2
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse2); }//now it is 2
             );
            Assert.AreEqual(tokenResponse2, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_RemoveItem_RenewCache()
        {
            var tokenResponse1 = await TokenResponseMock.GetValidResponseAsync("access_token_1", 10);
            var tokenResponse2 = await TokenResponseMock.GetValidResponseAsync("access_token_2", 10);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            //should cache it and returne tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);

            //remove
            accessTokenCacheManager.Remove("CacheKey");

            //tokenResponse1 should be removed
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(tokenResponse2); }//now it is tokenResponse2
             );
            Assert.AreEqual(tokenResponse2, tokenResponseFromCache);

        }
    }
}
