using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;

namespace IdentityServer4.HttpClientService.Infrastructure
{
    /// <summary>
    /// A <see cref="HttpRequestMessage"/> factory.
    /// </summary>
    public class HttpRequestMessageFactory : IHttpRequestMessageFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor of the  <see cref="HttpRequestMessageFactory"/>
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="HttpContextAccessor"/> object to access <see cref="HttpContext"/>. </param>
        public HttpRequestMessageFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds an <c>X-HttpClientService</c> header in the newly created <see cref="HttpRequestMessage"/> by copying the <c>X-HttpClientService</c> of the current request.
        /// If there is no current request or the current request does not contain a <c>X-HttpClientService</c> header, it creates a new one.
        /// </summary>
        /// <remarks>This is useful to track in the logs a series of cascading requests between services.</remarks>
        /// <returns>An <see cref="HttpRequestMessage"/> to be used by an <see cref="HttpClient"/>.</returns>
        public HttpRequestMessage CreateRequestMessage()
        {
            var request = new HttpRequestMessage();
            var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;

            if (headers != default)
            {
                if (headers.ContainsKey("X-HttpClientService") && !string.IsNullOrWhiteSpace(headers["X-HttpClientService"].ToString()))
                    request.Headers.Add("X-HttpClientService", headers["X-HttpClientService"].ToString());
                else
                    request.Headers.Add("X-HttpClientService", Guid.NewGuid().ToString());
            }
            else
                request.Headers.Add("X-HttpClientService", Guid.NewGuid().ToString());

            return request;
        }

    }
}