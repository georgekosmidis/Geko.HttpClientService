using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// A <see cref="HttpRequestMessage"/> factory.
    /// </summary>
    public class HttpRequestMessageFactoryDesktop : IHttpRequestMessageFactory
    {

        /// <summary>
        /// Constructor of the  <see cref="HttpRequestMessageFactory"/>
        /// </summary>
        public HttpRequestMessageFactoryDesktop()
        {
        }

        /// <summary>
        /// Creates and returns a new <see cref="HttpRequestMessage"/>
        /// </summary>
        /// <returns>An <see cref="HttpRequestMessage"/> to be used by an <see cref="HttpClient"/>.</returns>
        public HttpRequestMessage CreateRequestMessage()
        {
            return new HttpRequestMessage();           
        }

    }
}