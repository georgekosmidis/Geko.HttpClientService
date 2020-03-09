using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;
using System.Net;
using System.IO;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public static class IHttpClientFactoryMocks
    {
        public static IHttpClientFactory Get(HttpStatusCode httpStatusCode)
        {
            return GetHttpClientFactory(httpStatusCode, new StringContent(""));
        }

        public static IHttpClientFactory Get(HttpStatusCode httpStatusCode, Stream responseBody)
        {
            return GetHttpClientFactory(httpStatusCode, new StreamContent(responseBody));
        }

        public static IHttpClientFactory Get(HttpStatusCode httpStatusCode, string responseBody)
        {
            return GetHttpClientFactory(httpStatusCode, new StringContent(responseBody));
        }

        private static IHttpClientFactory GetHttpClientFactory(HttpStatusCode httpStatusCode, HttpContent responseBody)
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var clientHandlerStub = new DelegatingHandlerStub(httpStatusCode, responseBody);
            var client = new HttpClient(clientHandlerStub);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            return mockFactory.Object;
        }
    }
}
