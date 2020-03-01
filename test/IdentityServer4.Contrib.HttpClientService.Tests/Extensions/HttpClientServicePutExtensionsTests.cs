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
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServicePutExtensionsTests : TestBase
    {

        [TestMethod]
        public async Task HttpClientServicePut_StreamRequestStringResponse()
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


            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(this.ComplexTypeRequestString);
                writer.Flush();
                memoryStream.Position = 0;

                var result = await httpClientService.PutAsync<string>(
                    "http://localhost",
                    new StreamContent(memoryStream)
                );

                //Status/HttpResponseMessage
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

                //HttpRequestMessage
                Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
                Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

                //Body
                Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
                var sr = new StreamReader(result.BodyAsStream);
                Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
                Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

                httpClientService.Dispose();
            }
        }

        [TestMethod]
        public async Task HttpClientServicePut_StringRequestStringResponse()
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

            var result = await httpClientService.PutAsync<string>(
                "http://localhost",
                this.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

            httpClientService.Dispose();
        }

        [TestMethod]
        public async Task HttpClientServicePut_NoTypesDefined()
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

            var result = await httpClientService.PutAsync(
                "http://localhost",
                this.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

            httpClientService.Dispose();
        }

        [TestMethod]
        public async Task HttpClientServicePut_StringContentRequestStringResponse()
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

            var result = await httpClientService.PutAsync<string>(
                "http://localhost",
                new StringContent(this.ComplexTypeRequestString, Encoding.UTF32, "fake/type")
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());
            Assert.IsNotNull("utf-32", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.IsNotNull("fake/type", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

            httpClientService.Dispose();
        }


        [TestMethod]
        public async Task HttpClientServicePut_ComplexTypeRequestStringResponse()
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

            var result = await httpClientService.PutAsync<ComplexTypeRequest, string>(
                "http://localhost",
                new ComplexTypeRequest()
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

            httpClientService.Dispose();
        }
    }
}
