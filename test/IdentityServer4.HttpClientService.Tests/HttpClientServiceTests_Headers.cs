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
using System.IO;
using System.Collections.Generic;

namespace IdentityServer4.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_Headers
    {

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersAddedToRequest()
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

            var result = await httpClientService
                .AddHeader("x-test-header", "x-test-value")
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            httpClientService.Dispose();

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual("x-test-value", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
        }

    }
}
