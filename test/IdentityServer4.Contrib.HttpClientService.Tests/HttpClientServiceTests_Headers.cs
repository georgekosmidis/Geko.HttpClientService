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
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Test
{
    [TestClass]
    public class HttpClientServiceTests_Headers
    {

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersAddToRequest_ShouldAddOne()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", "x-test-value")
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual("x-test-value", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersAddListToRequest_ShouldAddList()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", new List<string> { "x-test-value-1", "x-test-value-2" })
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual("x-test-value-1", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
            Assert.AreEqual("x-test-value-2", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Last());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersAddSecondSameToRequest_ShouldAggregate()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", "x-test-value-1")
                .HeadersAdd("x-test-header", "x-test-value-2")
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual("x-test-value-1", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
            Assert.AreEqual("x-test-value-2", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Last());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersSet_ShouldReset()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", "x-test-value-1")
                .HeadersAdd(new Dictionary<string, string>()
                {
                    { "x-test-header", "x-test-value-2" }
                })
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual(2, result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Count());
            Assert.AreEqual("x-test-value-1", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
            Assert.AreEqual("x-test-value-2", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Last());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersSetList_ShouldNotReset()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", "x-test-value-1")
                .HeadersAdd(new Dictionary<string, List<string>>()
                {
                    { "x-test-header", new List<string>{ "x-test-value-2", "x-test-value-3" } }
                })
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header"));
            Assert.AreEqual(3, result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Count());
            Assert.AreEqual("x-test-value-1", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.First());
            Assert.AreEqual("x-test-value-3", result.HttpRequestMessge.Headers.First(x => x.Key == "x-test-header").Value.Last());
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersClear_ShouldReset()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header", "x-test-value-1")
                .HeadersClear()
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsFalse(result.HttpRequestMessge.Headers.Contains("x-test-header"));
        }

        [TestMethod]
        public async Task HttpClientServiceTests_Headers_HeadersRemove_ShouldRemoveOne()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            var result = await httpClientService
                .HeadersAdd("x-test-header-1", "x-test-value-1")
                .HeadersAdd("x-test-header-2", "x-test-value-2")
                .HeadersRemove("x-test-header-2")
                .SendAsync<string, object>(
                new Uri("http://localhost"),
                HttpMethod.Get,
                null
            );

            Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("x-test-header-1"));
            Assert.IsFalse(result.HttpRequestMessge.Headers.Contains("x-test-header-2"));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task HttpClientServiceTests_StringContentAndHeaders_ShouldFail()
        {
            var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

            await httpClientService
                 .HeadersAdd("Content-Type", "application/json; charset=utf-8")
                 .SendAsync<StringContent, string>(
                 new Uri("http://localhost"),
                 HttpMethod.Post,
                 new StringContent("request_content", Encoding.UTF8, "application/json")
             );
        }
    }
}
