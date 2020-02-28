using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.HttpClientService.Infrastructure;
using IdentityServer4.HttpClientService.Tests.Helpers;
using IdentityModel.Client;
using System.Net;
using Microsoft.Extensions.Options;
using IdentityServer4.HttpClientService.Models;
using System;

namespace IdentityServer4.HttpClientService.Tests.Infrastructure
{
    [TestClass]
    public class AccessTokenCacheManagerTests
    {

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_RetrieveNew()
        {
            var cachedTokenResponse = await TokenResponseMock.GetValidResponseAsync("cached_access_token", 3600);
            var liveTokenResponse = await TokenResponseMock.GetValidResponseAsync("live_access_token", 3600);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                IMemoryCacheMocks.Get(cachedTokenResponse, false)
            );

            var accessToken = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                 () => { return Task.FromResult(liveTokenResponse); }
             );
            Assert.AreEqual(liveTokenResponse, accessToken);
        }

        [TestMethod]
        public async Task AccessTokenCacheManager_AddOrGet_ReturneCached()
        {
            var cachedTokenResponse = await TokenResponseMock.GetValidResponseAsync("cached_access_token", 3600);
            var liveTokenResponse = await TokenResponseMock.GetValidResponseAsync("live_access_token", 3600);

            var accessTokenCacheManager = new TokenResponseCacheManager(
                IMemoryCacheMocks.Get(cachedTokenResponse, true)
            );

            var accessToken = await accessTokenCacheManager.AddOrGetExistingAsync(
                "CacheKey",
                () => { return Task.FromResult(liveTokenResponse); }
             );
            Assert.AreEqual(cachedTokenResponse.AccessToken, accessToken.AccessToken);
        }
    }
}
