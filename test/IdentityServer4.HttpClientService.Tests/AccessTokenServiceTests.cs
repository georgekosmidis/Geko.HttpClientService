using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.HttpClientService.Models;
using Microsoft.Extensions.Options;
using IdentityServer4.HttpClientService;
using IdentityServer4.HttpClientService.Tests.Helpers;
using IdentityServer4.HttpClientService.Infrastructure;
using System.Net;
using IdentityModel.Client;
using System;

namespace IdentityServer4.HttpClientService.Test
{
    [TestClass]
    public class AccessTokenServiceTests
    {

        [TestMethod]
        public async Task AccessTokenService_GetAccessToken()
        {
            var cachedTokenResponse = await TokenResponseMock.GetValidResponseAsync("cached_access_token", 0);
            var request = new TokenResponseService(
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK,
                @"{
                    ""access_token"": ""live_access_token"",
                    ""expires_in"": 10,
                    ""token_type"": ""Bearer"",
                    ""custom"":  ""liveObject""
                }"),
                IAccessTokenCacheManagerMocks.Get(cachedTokenResponse)
            );

            var tokenServiceOptions = Options.Create(new DefaultClientCredentialOptions
            {
                Address = "http://localhost/" + Guid.NewGuid()
            });

            var accessToken = await request.GetTokenResponseAsync(tokenServiceOptions);
            Assert.AreEqual("cached_access_token", accessToken.AccessToken);
        }

    }
}
