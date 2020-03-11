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
    public class HttpClientServiceTests_ResponseTypes
    {
        [TestMethod]
        public async Task HttpClientServiceTests_ComplexResponseTypeAsType()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.SendAsync<object, ComplexTypes.ComplexTypeResponse>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

            result.HttpRequestMessge.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestInt, result.BodyAsType.TestInt);

            Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestBool, result.BodyAsType.TestBool);
        }

        [TestMethod]
        public async Task HttpClientServiceTests_ComplexResponseTypeAsString()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.SendAsync<object, string>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

            result.HttpRequestMessge.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);

        }

        [TestMethod]
        public async Task HttpClientServiceTests_PrimitiveResponseTypeAsType()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "-123", true);

            var result = await httpClientService.SendAsync<object, int>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

            result.HttpRequestMessge.Dispose();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(-123, result.BodyAsType);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task HttpClientServiceTests_WrongType()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_as_string", true);

            await httpClientService.SendAsync<object, int>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );
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

                var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, memoryStream, true);

                result = await httpClientService.SendAsync<object, Stream>(
                    new Uri("http://localhost"),
                    HttpMethod.Get,
                    null
                );

                result.HttpRequestMessge.Dispose();
            }

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var sr = new StreamReader(result.BodyAsStream);
            Assert.AreEqual("test_body_as_stream", sr.ReadToEnd());
        }
    }
}
