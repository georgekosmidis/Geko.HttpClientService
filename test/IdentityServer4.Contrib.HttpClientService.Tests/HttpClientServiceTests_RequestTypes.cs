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
    public class HttpClientServiceTests_RequestTypes
    {

        [TestMethod]
        public async Task HttpClientServiceTests_ComplexRequestType_ShouldSerializeAndSent()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

            var result = await httpClientService.SendAsync<ComplexTypes.ComplexTypeRequest, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    ComplexTypes.ComplexTypeRequestInstance
                );

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("application/json", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-8", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), requestBody.ToLower());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_TypeContentRequestTypeDefault_ShouldSerializeAndSent()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

            var result = await httpClientService.SendAsync<TypeContent<ComplexTypes.ComplexTypeRequest>, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new TypeContent<ComplexTypes.ComplexTypeRequest>(ComplexTypes.ComplexTypeRequestInstance)
                );

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("application/json", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-8", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), requestBody.ToLower());
        }


        [TestMethod]
        public async Task HttpClientServiceTests_TypeContentRequestTypeWithHeaders_ShouldSerializeAndSent()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

            var result = await httpClientService.SendAsync<TypeContent<ComplexTypes.ComplexTypeRequest>, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Post,
                    new TypeContent<ComplexTypes.ComplexTypeRequest>(ComplexTypes.ComplexTypeRequestInstance, Encoding.UTF32, "some/type")
                );

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            var requestBody = await result.HttpRequestMessge.Content.ReadAsStringAsync();
            Assert.AreEqual("some/type", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-32", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), requestBody.ToLower());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_PrimitiveRequestType_ShouldSerializeAndSent()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

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
        public async Task HttpClientServiceTests_StringContentRequestTypeWithHeaders_ShouldSerializeAndSent()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

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
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.Created, "body_of_response", true);

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
