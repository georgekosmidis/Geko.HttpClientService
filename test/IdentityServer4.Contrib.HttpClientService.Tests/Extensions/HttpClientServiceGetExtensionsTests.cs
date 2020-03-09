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
    public class HttpClientServiceGetExtensionsTests
    {

        [TestMethod]
        public async Task HttpClientServiceGet_NoTypedResponse_ShouldBeResponseString()
        {

            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.GetAsync("http://localhost");

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Get, result.HttpRequestMessge.Method);
            Assert.IsNull(result.HttpRequestMessge.Content);

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
        }

        [TestMethod]
        public async Task HttpClientServiceGet_TypedResponse_ShouldBeResponseType()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

            var result = await httpClientService.GetAsync<ComplexTypes.ComplexTypeResponse>("http://localhost");

            //Status/HttpResponseMessage
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);

            //HttpRequestMessage
            Assert.AreEqual(HttpMethod.Get, result.HttpRequestMessge.Method);
            Assert.IsNull(result.HttpRequestMessge.Content);

            //Body
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);

            Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestInt, result.BodyAsType.TestInt);

            Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
            Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestBool, result.BodyAsType.TestBool);
        }

    }
}
