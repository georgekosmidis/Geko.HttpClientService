using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using System;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure.Tests
{
    [TestClass]
    public class HttpRequestMessageFactoryTests
    {
        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldNotContainXHeader()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions
            {
                HeaderCollerationIdActive = false,
                HeaderCollerationName = "X-ShouldNotBeThere"
            };

            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(httpClientServiceOptions)
            ).CreateRequestMessage();

            Assert.AreEqual(0, message.Headers.Count());
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldReturnsClientWithXHeader()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions();

            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(httpClientServiceOptions)
            ).CreateRequestMessage();

            Assert.IsTrue(message.Headers.Contains(httpClientServiceOptions.HeaderCollerationName));
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldAddNewXHeaderForEachRequestMessage()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions();

            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(httpClientServiceOptions)
            );

            var header1 = request.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
            var header2 = request.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();

            Assert.AreNotEqual(header1, header2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldUseSameXHeaderForSameInsance()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions();

            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(httpClientServiceOptions)
            ).CreateRequestMessage();

            var header1 = message.Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
            var header2 = message.Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();

            Assert.AreEqual(header1, header2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldCopyXHeader()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions();

            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("X-HttpClientService-previous-value"),
                Options.Create(httpClientServiceOptions)
            ).CreateRequestMessage();


            var header = message.Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
            Assert.AreEqual("X-HttpClientService-previous-value", header);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldCopyXHeaderIfNotEmptyString()
        {
            var httpClientServiceOptions = new HttpClientServiceOptions();

            var request1 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("  "),//2 spaces
                Options.Create(httpClientServiceOptions)
            );

            var request2 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(""),//empty
                Options.Create(httpClientServiceOptions)
            );

            var header1 = request1.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
            Assert.AreNotEqual("  ", header1);

            var header2 = request2.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
            Assert.AreNotEqual("", header2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ParallelismWithDifferentRequestMessage_ShouldHaveDifferentXHeader()
        {
            if (Environment.ProcessorCount == 1)
                throw new InvalidOperationException("Concurreny test with 1 processor are not possible!");

            var httpClientServiceOptions = new HttpClientServiceOptions();
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(httpClientServiceOptions)
            );

            Parallel.For(0, Environment.ProcessorCount * 2, i =>
            {
                var header1 = request.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
                var header2 = request.CreateRequestMessage().Headers.First(x => x.Key == httpClientServiceOptions.HeaderCollerationName).Value.First();
                Assert.AreNotEqual(header1, header2);
            });
        }
    }
}
