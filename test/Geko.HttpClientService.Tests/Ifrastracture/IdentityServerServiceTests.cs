using Geko.HttpClientService.Models;
using Geko.HttpClientService.Tests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Infrastructure.Tests;

[TestClass]
public class IdentityServerServiceTests
{
    [TestMethod]
    public async Task IdentityServerService_GetTokenResponse_ShouldReturnAccessToken()
    {
        var identityServerService = new IdentityServerService(
            new IdentityServerHttpClientSelector(
                new List<IIdentityServerHttpClient> {
                        {
                            new ClientCredentialsHttpClient(
                                IHttpClientFactoryMocks.Get(
                                    HttpStatusCode.OK,
                                    TokenResponseObjects.GetValidTokenResponseString("live_access_token", 10)
                                ).CreateClient("test")
                            )
                        }
                }
            ),
            new TokenResponseCacheManager(
                new MemoryCache(
                    Options.Create(new MemoryCacheOptions())
                )
            )
        );

        var tokenServiceOptions = new ClientCredentialsOptions
        {
            Address = "http://localhost/" + Guid.NewGuid(),
            ClientId = "ClientId",
            ClientSecret = "secret",
            Scope = "scope"
        };

        var accessToken = await identityServerService.GetTokenResponseAsync(tokenServiceOptions);
        Assert.AreEqual("live_access_token", accessToken.AccessToken);
    }

}
