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

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure.Tests
{
    [TestClass]
    public class TokenResponseCacheManagerTests
    {
        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_EmptyCache_ShouldRetrieveNew()
        {
            var tokenResponse = await TokenResponseObjects.GetValidTokenResponseAsync("access_token", 5);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey_" + Guid.NewGuid(),
                () => { return Task.FromResult(tokenResponse); }
             );

            Assert.AreEqual(tokenResponse, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_ShouldReturnCached()
        {
            var tokenResponse1 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_1", 5);
            var tokenResponse2 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_2", 5);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            var cacheKey = "CacheKey_" + Guid.NewGuid();

            //should cache it and return tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);

            //should not cache it and return tokenResponse1
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse2); }//now it is 2
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_ShouldRenewExpiredItem()
        {
            var tokenResponse1 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_1", 1);
            var tokenResponse2 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_2", 1);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            var cacheKey = "CacheKey_" + Guid.NewGuid();

            //should cache it and returne tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);

            //sleep for cache expiration
            Thread.Sleep(1000);

            //should cache it and return tokenResponse2
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse2); }//now it is 2
             );
            Assert.AreEqual(tokenResponse2, tokenResponseFromCache);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_NotEmptyCache_RemoveItem_ShouldRenewCache()
        {
            var tokenResponse1 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_1", 5);
            var tokenResponse2 = await TokenResponseObjects.GetValidTokenResponseAsync("access_token_2", 5);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            );

            var cacheKey = "CacheKey_" + Guid.NewGuid();

            //should cache it and returne tokenResponse1
            var tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse1); }
             );
            Assert.AreEqual(tokenResponse1, tokenResponseFromCache);

            //remove
            accessTokenCacheManager.Remove(cacheKey);

            //tokenResponse1 should be removed
            tokenResponseFromCache = await accessTokenCacheManager.AddOrGetExistingAsync(
                cacheKey,
                () => { return Task.FromResult(tokenResponse2); }//now it is tokenResponse2
             );
            Assert.AreEqual(tokenResponse2, tokenResponseFromCache);

        }
    }
}
