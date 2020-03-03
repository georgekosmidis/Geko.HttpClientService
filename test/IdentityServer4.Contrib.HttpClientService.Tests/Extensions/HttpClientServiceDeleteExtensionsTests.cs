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
using IdentityServer4.Contrib.HttpClientService.Extensions;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceDeleteExtensionsTests : TestBase
    {

        [TestMethod]
        public async Task HttpClientServiceDelete_NoTypedResponse()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.NoContent, "").CreateClient()
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

            var result = await httpClientService.DeleteAsync("http://localhost");

            httpClientService.Dispose();

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NoContent, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Delete, result.HttpRequestMessge.Method);
            Assert.IsNull(result.HttpRequestMessge.Content);

            //Body
            Assert.IsTrue(String.IsNullOrEmpty(result.BodyAsString));
            Assert.IsTrue(String.IsNullOrEmpty(result.BodyAsType));
            var sr = new StreamReader(result.BodyAsStream);
            Assert.IsTrue(String.IsNullOrEmpty(sr.ReadToEnd()));
        }

        [TestMethod]
        public async Task HttpClientServiceDelete_TypedResponse()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, this.ComplexTypeResponseString).CreateClient()
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

            var result = await httpClientService.DeleteAsync<ComplexTypeResponse>("http://localhost");

            httpClientService.Dispose();

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Delete, result.HttpRequestMessge.Method);
            Assert.IsNull(result.HttpRequestMessge.Content);

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());

            Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
            Assert.AreEqual(new ComplexTypeResponse().TestInt, result.BodyAsType.TestInt);

            Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
            Assert.AreEqual(new ComplexTypeResponse().TestBool, result.BodyAsType.TestBool);

        }

    }
}
