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
    public class HttpClientServiceTests_ResponseTypes : TestBase
    {
        [TestMethod]
        public async Task HttpClientServiceTests_ComplexResponseTypeAsType()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, this.ComplexTypeResponseString),
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

            var result = await httpClientService.SendAsync<object, ComplexTypeResponse>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

            httpClientService.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
            Assert.AreEqual(new ComplexTypeResponse().TestInt, result.BodyAsType.TestInt);

            Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
            Assert.AreEqual(new ComplexTypeResponse().TestBool, result.BodyAsType.TestBool);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_ComplexResponseTypeAsString()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, this.ComplexTypeResponseString),
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

            httpClientService.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

        }

        [TestMethod]
        public async Task HttpClientServiceTests_PrimitiveResponseTypeAsType()
        {
            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "-123"),
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

            var result = await httpClientService.SendAsync<object, int>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

            httpClientService.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(-123, result.BodyAsType);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task HttpClientServiceTests_WrongType()
        {
            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("section_data"),
                IHttpClientFactoryMocks.Get(HttpStatusCode.OK, "string_as_body"),
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

            try
            {
                await httpClientService.SendAsync<object, int>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );
            }
            finally
            {
                httpClientService.Dispose();
            }

        }

        [TestMethod]
        public async Task HttpClientServiceTests_StreamAsStream()
        {
            ResponseObject<Stream> result;
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write("test_body_as_stream");
                writer.Flush();
                memoryStream.Position = 0;

                var httpClientService = new HttpClientServiceFactory(
                    IConfigurationMocks.Get("section_data"),
                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK, memoryStream),
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

                result = await httpClientService.SendAsync<object, Stream>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

                httpClientService.Dispose();
            }

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual("test_body_as_stream", sr.ReadToEnd());
        }
    }
}
