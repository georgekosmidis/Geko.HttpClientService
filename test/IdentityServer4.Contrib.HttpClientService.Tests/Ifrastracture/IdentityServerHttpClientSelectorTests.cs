using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using System.Net;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure.Tests
{
    [TestClass]
    public class IdentityServerHttpClientSelectorTests
    {

        [TestMethod]
        public async Task IdentityServerHttpClientSelector_Get_ShouldSelectClientCredentials()
        {
            var identityServerService = new IdentityServerService(
                    new IdentityServerHttpClientSelector(
                        new List<IIdentityServerHttpClient> {
                            {
                                new ClientCredentialsHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "{\"access_token\": \"client_credentials_access_token\"}").CreateClient("test")
                                )
                            },
                            {
                                new PasswordHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "{\"access_token\": \"password_access_token\"}").CreateClient("test")
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
            Assert.AreEqual("client_credentials_access_token", accessToken.AccessToken);
        }

        [TestMethod]
        public async Task IdentityServerHttpClientSelector_Get_ShouldSelectPassword()
        {
            var identityServerService = new IdentityServerService(
                    new IdentityServerHttpClientSelector(
                        new List<IIdentityServerHttpClient> {
                            {
                                new ClientCredentialsHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "{\"access_token\": \"client_credentials_access_token\"}").CreateClient("test")
                                )
                            },
                            {
                                new PasswordHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "{\"access_token\": \"password_access_token\"}").CreateClient("test")
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

            var tokenServiceOptions = new PasswordOptions
            {
                Address = "http://localhost/" + Guid.NewGuid(),
                ClientId = "ClientId",
                ClientSecret = "secret",
                Scope = "scope",
                Username = "username",
                Password = "password"
            };

            var accessToken = await identityServerService.GetTokenResponseAsync(tokenServiceOptions);
            Assert.AreEqual("password_access_token", accessToken.AccessToken);
        }
    }
}
