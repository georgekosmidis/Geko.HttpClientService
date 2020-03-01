using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using System.Net;
using IdentityModel.Client;
using System;
using System.IO;
using System.Collections.Generic;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_Authorization
    {

        [TestMethod]
        public async Task HttpClientServiceTests_Authorization_NoIdentityServerOptions()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "body_of_response"),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
                ),
                new TokenResponseService(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK),
                    IAccessTokenCacheManagerMocks.Get(
                        await TokenResponseMock.GetValidResponseAsync("access_token", 3600)
                    )
                )
            ).CreateHttpClientService();

            var result = await httpClientService.SendAsync<object, string>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsFalse(result.HttpRequestMessge.Headers.Contains("Authorization"));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("body_of_response", result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Authorization_WithIdentityServerOptions()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "body_of_response"),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
                ),
                new TokenResponseService(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK),
                    IAccessTokenCacheManagerMocks.Get(
                        await TokenResponseMock.GetValidResponseAsync("access_token", 3600)
                    )
                )
            ).CreateHttpClientService();

            httpClientService.SetIdentityServerOptions(
                Options.Create(
                    new DefaultClientCredentialOptions
                    {
                        Address = "http://localhost",
                        ClientId = "ClientId",
                        ClientSecret = "ClientSecret"
                    }
                )
            );

            var result = await httpClientService.SendAsync<object, string>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            httpClientService.Dispose();

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("Authorization"));
            Assert.AreEqual("Bearer", result.HttpRequestMessge.Headers.Authorization.Scheme);
            Assert.AreEqual("access_token", result.HttpRequestMessge.Headers.Authorization.Parameter);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("body_of_response", result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Authorization_WithWrongIdentityServerOptions()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "body_of_response"),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
                ),
                new TokenResponseService(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK),
                    IAccessTokenCacheManagerMocks.Get(
                        await TokenResponseMock.GetErrorResponseAsync("invalid_client")
                    )
                )
            ).CreateHttpClientService();

            httpClientService.SetIdentityServerOptions(
                Options.Create(
                    new DefaultClientCredentialOptions
                    {
                        Address = "http://localhost",
                        ClientId = "ClientId",
                        ClientSecret = "ClientSecret"
                    }
                )
            );

            var result = await httpClientService.SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            httpClientService.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("invalid_client", result.Error.Trim());
        }
    }
}
