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
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace IdentityServer4.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_NoSuccessStatusCodes
    {

        [TestMethod]
        public async Task HttpClientServiceTests_Returns400()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.BadRequest, "body_of_response"),
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

            var result = await httpClientService.SendAsync<string, object>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );


            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Bad Request", result.Error);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Returns404()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.NotFound, "body_of_response"),
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

            var result = await httpClientService.SendAsync<string, object>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );


            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Not Found", result.Error);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Returns500()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.InternalServerError, "body_of_response"),
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

            var result = await httpClientService.SendAsync<string, object>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );


            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Internal Server Error", result.Error);
        }
    }
}
