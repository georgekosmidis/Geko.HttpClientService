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
    public class HttpClientServicePatchExtensionsTests : TestBase
    {

        [TestMethod]
        public async Task HttpClientServicePatch_StreamRequestStringResponse_ShouldBeResponseString()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
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


            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(this.ComplexTypeRequestString);
                writer.Flush();
                memoryStream.Position = 0;

                var result = await httpClientService.PatchAsync<string>(
                    "http://localhost",
                    new StreamContent(memoryStream)
                );

                //Status/HttpResponseMessage
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

                //HttpRequestMessage
                Assert.AreEqual(HttpMethod.Patch, result.HttpRequestMessge.Method);
                Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

                //Body
                Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
                //var sr = new StreamReader(result.BodyAsStream);
                //Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
                Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);

                result.HttpRequestMessge.Dispose();
            }
        }

        [TestMethod]
        public async Task HttpClientServicePatch_NoTypesDefined_ShouldBeResponseString()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
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

            var result = await httpClientService.PatchAsync(
                "http://localhost",
                this.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Patch, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            //var sr = new StreamReader(result.BodyAsStream);
            //Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServicePatch_StringRequestStringResponse_ShouldBeResponseString()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
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

            var result = await httpClientService.PatchAsync<string>(
                "http://localhost",
                this.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Patch, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            //var sr = new StreamReader(result.BodyAsStream);
            //Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServicePatch_StringContentRequestTypedResponse_ShouldBeResponseType()
        {

            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
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

            var result = await httpClientService.PatchAsync<string>(
                "http://localhost",
                new StringContent(this.ComplexTypeRequestString, Encoding.UTF32, "fake/type")
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Patch, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());
            Assert.IsNotNull("utf-32", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.IsNotNull("fake/type", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            //var sr = new StreamReader(result.BodyAsStream);
            //Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);
        }


        [TestMethod]
        public async Task HttpClientServicePatch_ComplexTypeRequestStringResponse_ShouldBeResponseString()
        {
            var httpClientService = new HttpClientServiceFactory(
                IConfigurationMocks.Get("key", "section_data"),
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

            var result = await httpClientService.PatchAsync<ComplexTypeRequest, string>(
                "http://localhost",
                new ComplexTypeRequest()
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Patch, result.HttpRequestMessge.Method);
            Assert.IsNotNull(this.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsString);
            //var sr = new StreamReader(result.BodyAsStream);
            //Assert.AreEqual(this.ComplexTypeResponseString, sr.ReadToEnd());
            Assert.AreEqual(this.ComplexTypeResponseString, result.BodyAsType);
        }
    }
}
