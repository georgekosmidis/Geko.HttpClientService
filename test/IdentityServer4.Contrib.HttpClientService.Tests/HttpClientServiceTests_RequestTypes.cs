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
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_RequestTypes : TestBase
    {

        [TestMethod]
        public async Task HttpClientServiceTests_ComplexRequestType_ShouldSerializeAndSent()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.Created, "body_of_response").CreateClient()
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

            var result = await httpClientService.SendAsync<ComplexTypeRequest, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new ComplexTypeRequest()
                );

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("application/json", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-8", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual(this.ComplexTypeRequestString.ToLower(), requestBody.ToLower());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_PrimitiveRequestType_ShouldSerializeAndSent()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.Created, "body_of_response").CreateClient()
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

            var result = await httpClientService.SendAsync<int, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    123
                );

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("text/plain", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-8", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual("123", requestBody);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_StringContentRequestType_ShouldSerializeAndSent()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.Created, "body_of_response").CreateClient()
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

            var result = await httpClientService.SendAsync<StringContent, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new StringContent("request_body", Encoding.UTF32, "some/type")
                );            

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("some/type", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-32", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual("request_body", requestBody);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_StreamContentRequestType_ShouldSentAsStream()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(HttpStatusCode.Created, "body_of_response").CreateClient()
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

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write("test_body_as_stream");
                writer.Flush();
                memoryStream.Position = 0;

                var result = await httpClientService.SendAsync<StreamContent, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new StreamContent(memoryStream)
                );

                Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

                var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
                Assert.AreEqual("test_body_as_stream", requestBody);

                result.HttpRequestMessge.Dispose();
            }
        }
    }
}
