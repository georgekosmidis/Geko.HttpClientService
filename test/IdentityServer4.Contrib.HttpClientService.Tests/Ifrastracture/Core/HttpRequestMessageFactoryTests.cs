using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using System;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure.Tests
{
    [TestClass]
    public class HttpRequestMessageFactoryTests
    {
        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldReturnsClientWithXHeader()
        {
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            ).CreateRequestMessage();
            
            Assert.IsTrue(request.Headers.Contains("X-HttpClientService"));
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldAddNewXHeaderForEachRequestMessage()
        {
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            );

            var call1 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            var call2 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            
            Assert.AreNotEqual(call1, call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldUseSameXHeaderForSameInsance()
        {
            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            ).CreateRequestMessage();

            var call1 = message.Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            var call2 = message.Headers.First(x => x.Key == "X-HttpClientService").Value.First();

            Assert.AreEqual(call1, call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldCopyXHeader()
        {
            var message = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("X-HttpClientService-previous-value")
            ).CreateRequestMessage();


            var call = message.Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            Assert.AreEqual("X-HttpClientService-previous-value", call);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ShouldCopyXHeaderIfNotEmptyString()
        {
            var request1 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("  ")//2 spaces
            );

            var request2 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("")//empty
            );

            var call1 = request1.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            Assert.AreNotEqual("  ", call1);

            var call2 = request2.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            Assert.AreNotEqual("", call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ParallelismWithDifferentRequestMessage_ShouldHaveDifferentXHeader()
        {
            if (Environment.ProcessorCount == 1)
                throw new InvalidOperationException("Concurreny test with 1 processor are not possible!");

            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            );

            Parallel.For(0, Environment.ProcessorCount * 2, i => {
                var call1 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
                var call2 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
                Assert.AreNotEqual(call1, call2);
            });
        }
    }
}
