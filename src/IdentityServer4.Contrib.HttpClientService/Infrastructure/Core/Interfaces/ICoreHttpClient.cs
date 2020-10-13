using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Abstraction for a typed <see cref="HttpClient"/>.
    /// </summary>
    public interface ICoreHttpClient
    {
        /// <summary>
        /// Sends an <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="httpRequestMessage">The <see cref="HttpResponseMessage"/> to be sent</param>
        /// <returns>An <see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);

        /// <summary>
        /// Set the timeout for the next request.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        void SetTimeout(TimeSpan timeout);
    }
}