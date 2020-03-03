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
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_Headers
    {

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersAddedToRequest()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "body_of_response").CreateClient()
                ),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
                ),
                new TokenResponseService(
                    new IdentityServerHttpClient(
                        IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                    ),
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task HttpClientServiceTests_StringContentAndHeaders_HeadersAddedToRequest()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "body_of_response").CreateClient()
                ),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
                ),
                new TokenResponseService(
                    new IdentityServerHttpClient(
                        IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                    ),
                    IAccessTokenCacheManagerMocks.Get(
                        await TokenResponseMock.GetValidResponseAsync("access_token", 3600)
                    )
                )
            ).CreateHttpClientService();

            try
            {
                var result = await httpClientService
                    .AddHeader("Content-Type", "Misused header name")
                    .SendAsync<StringContent, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new StringContent("request_content", Encoding.UTF8, "application/json")
                );
            }
            finally
            {
                httpClientService.Dispose();
            }
        }

    }
}
