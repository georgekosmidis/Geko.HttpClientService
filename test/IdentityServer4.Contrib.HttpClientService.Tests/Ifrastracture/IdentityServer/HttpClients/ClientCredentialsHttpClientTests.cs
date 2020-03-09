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
    public class ClientCredentialsHttpClientTests
    {
        [TestMethod]
        public async Task ClientCredentialsHttpClient_GetTokenResponseAsync_ShouldReturnAccessToken()
        {
            var httpClient = new ClientCredentialsHttpClient(
                IHttpClientFactoryMocks.Get(
                    HttpStatusCode.OK,
                    TokenResponseObjects.GetValidTokenResponseString("access_token", 10)
                ).CreateClient("test")
            );

            var clientCredentialOptions = new ClientCredentialOptions
            {
                Address = "http://localhost/" + Guid.NewGuid(),
                ClientId = "ClientId",
                ClientSecret = "secret",
                Scope = "scope"
            };

            var tokenResponse = await httpClient.GetTokenResponseAsync(clientCredentialOptions);
            Assert.AreEqual(HttpStatusCode.OK, tokenResponse.HttpStatusCode);
            Assert.AreEqual("access_token", tokenResponse.AccessToken);
        }

        [TestMethod]
        public void ClientCredentialsHttpClient_GetCacheKey_ShouldReturnCorrectHash()
        {
            var httpClient = new ClientCredentialsHttpClient(
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient("test")
            );

            var clientCredentialOptions = new ClientCredentialOptions
            {
                Address = "http://localhost/" + Guid.NewGuid(),
                ClientId = "ClientId",
                ClientSecret = "secret",
                Scope = "scope"
            };

            var cacheKey = httpClient.GetCacheKey(clientCredentialOptions);
            var hash = (clientCredentialOptions.Address + clientCredentialOptions.ClientId + clientCredentialOptions.Scope).GetHashCode().ToString();

            Assert.AreEqual(hash, cacheKey);
        }

        [TestMethod]
        public void ClientCredentialsHttpClient_HttpClientOptionsType_ShouldBeCorrect()
        {
            var httpClient = new ClientCredentialsHttpClient(
                IHttpClientFactoryMocks.Get(
                    HttpStatusCode.OK,
                    TokenResponseObjects.GetValidTokenResponseString("access_token", 10)
                ).CreateClient("test")
            );

            Assert.AreEqual(typeof(ClientCredentialOptions), httpClient.HttpClientOptionsType);
        }
    }
}
