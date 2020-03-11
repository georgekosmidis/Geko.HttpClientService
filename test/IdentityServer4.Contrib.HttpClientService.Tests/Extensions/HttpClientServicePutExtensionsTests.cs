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
    public class HttpClientServicePutExtensionsTests
    {

        [TestMethod]
        public async Task HttpClientServicePut_StreamRequestStringResponse_ShouldBeResponseString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(ComplexTypes.ComplexTypeRequestString);
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
                Assert.IsNotNull(ComplexTypes.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

                //Body
                Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
                Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
            }
        }

        [TestMethod]
        public async Task HttpClientServicePut_StringRequestStringResponse_ShouldBeResponseString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.PutAsync<string>(
                "http://localhost",
                ComplexTypes.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(ComplexTypes.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServicePut_NoTypesDefined_ShouldBeResponseString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.PutAsync(
                "http://localhost",
                ComplexTypes.ComplexTypeRequestString
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(ComplexTypes.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServicePut_StringContentRequestStringResponse_ShouldBeResponseString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.PutAsync<string>(
                "http://localhost",
                new StringContent(ComplexTypes.ComplexTypeRequestString, Encoding.UTF32, "fake/type")
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(ComplexTypes.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());
            Assert.IsNotNull("utf-32", result.HttpRequestMessge.Content.Headers.ContentType.CharSet);
            Assert.IsNotNull("fake/type", result.HttpRequestMessge.Content.Headers.ContentType.MediaType);

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
        }


        [TestMethod]
        public async Task HttpClientServicePut_ComplexTypeRequestStringResponse_ShouldBeResponseString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.PutAsync<ComplexTypes.ComplexTypeRequest, string>(
                "http://localhost",
                ComplexTypes.ComplexTypeRequestInstance
            );

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Put, result.HttpRequestMessge.Method);
            Assert.IsNotNull(ComplexTypes.ComplexTypeRequestString, await result.HttpRequestMessge.Content.ReadAsStringAsync());

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
        }
    }
}
