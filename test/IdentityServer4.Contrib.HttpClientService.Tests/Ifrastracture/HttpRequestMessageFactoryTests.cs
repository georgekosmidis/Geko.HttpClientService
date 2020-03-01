using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Infrastructure
{
    [TestClass]
    public class HttpRequestMessageFactoryTests
    {
        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_ReturnsClientWithXHeader()
        {
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            ).CreateRequestMessage();
            
            Assert.IsTrue(request.Headers.Contains("X-HttpClientService"));
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_NewXHeaderForEachRequestMessage()
        {
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            );

            var call1 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            var call2 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            
            Assert.AreNotEqual(call1, call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_CopyXHeader()
        {
            var request1 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get("test")
            );

            var request2 = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            );

            var call1 = request1.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
            Assert.AreEqual("test", call1);

            var call2 = request2.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();            
            Assert.AreNotEqual("test", call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_CopyXHeaderIfNotEmptyString()
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
            Assert.AreNotEqual("  ", call2);
        }

        [TestMethod]
        public void HttpRequestMessageFactory_CreateRequestMessage_Parallelism()
        {
            var request = new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get()
            );

            Parallel.For(0, 1000, i => {
                var call1 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
                var call2 = request.CreateRequestMessage().Headers.First(x => x.Key == "X-HttpClientService").Value.First();
                Assert.AreNotEqual(call1, call2);
            });
        }
    }
}
