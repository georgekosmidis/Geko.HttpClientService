using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Models;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Ifrastracture.Core
{
    [TestClass]
    public class PasswordHttpClientTests
    {
        [TestMethod]
        public async Task PasswordHttpClient_GetTokenResponseAsync_ShouldReturnAccessToken()
        {
            var httpClient = new PasswordHttpClient(
                IHttpClientFactoryMocks.Get(
                    HttpStatusCode.OK,
                    TokenResponseObjects.GetValidTokenResponseString("access_token", 10)
                ).CreateClient("test")
            );

            var passwordOptions = new PasswordOptions
            {
                Address = "http://localhost/" + Guid.NewGuid(),
                ClientId = "ClientId",
                ClientSecret = "secret",
                Scope = "scope",
                Username = "username",
                Password = "password"
            };

            var tokenResponse = await httpClient.GetTokenResponseAsync(passwordOptions);
            Assert.AreEqual(HttpStatusCode.OK, tokenResponse.HttpStatusCode);
            Assert.AreEqual("access_token", tokenResponse.AccessToken);
        }

        [TestMethod]
        public void PasswordHttpClient_GetCacheKey_ShouldReturnCorrectHash()
        {
            var httpClient = new PasswordHttpClient(
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient("test")
            );

            var clientCredentialOptions = new PasswordOptions
            {
                Address = "http://localhost/" + Guid.NewGuid(),
                ClientId = "ClientId",
                ClientSecret = "secret",
                Scope = "scope",
                Username = "username",
                Password = "password"
            };

            var cacheKey = httpClient.GetCacheKey(clientCredentialOptions);
            var hash = (clientCredentialOptions.Address + clientCredentialOptions.ClientId + clientCredentialOptions.Scope + clientCredentialOptions.Username).GetHashCode().ToString();

            Assert.AreEqual(hash, cacheKey);
        }

        [TestMethod]
        public void PasswordHttpClient_HttpClientOptionsType_ShouldBeCorrect()
        {
            var httpClient = new PasswordHttpClient(
                IHttpClientFactoryMocks.Get(
                    HttpStatusCode.OK,
                    TokenResponseObjects.GetValidTokenResponseString("access_token", 10)
                ).CreateClient("test")
            );

            Assert.AreEqual(typeof(PasswordOptions), httpClient.HttpClientOptionsType);
        }
    }
}
