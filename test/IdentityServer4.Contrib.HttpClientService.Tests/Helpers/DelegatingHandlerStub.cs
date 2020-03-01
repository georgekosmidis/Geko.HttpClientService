using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<System.Net.Http.HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub(HttpStatusCode httpStatusCode)
        {
            _handlerFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(httpStatusCode));
        }

        //public DelegatingHandlerStub(HttpStatusCode httpStatusCode, StreamContent streamContent)
        //{
        //    var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
        //    httpResponseMessage.Content = streamContent;
        //    _handlerFunc = (request, cancellationToken) => Task.FromResult(httpResponseMessage);
        //}

        public DelegatingHandlerStub(HttpStatusCode httpStatusCode, HttpContent httpContent)
        {
            var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
            httpResponseMessage.Content = httpContent;
            _handlerFunc = (request, cancellationToken) => Task.FromResult(httpResponseMessage);
        }

        public DelegatingHandlerStub(Func<System.Net.Http.HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }
    }
}
