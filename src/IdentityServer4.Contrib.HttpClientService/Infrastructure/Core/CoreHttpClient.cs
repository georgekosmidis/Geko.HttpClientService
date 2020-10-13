using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Models;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Implementation for a typed <see cref="HttpClient"/>.
    /// </summary>
    public class CoreHttpClient : ICoreHttpClient
    {
        private readonly HttpClient _httpClient;
        private TimeSpan Timeout { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Constructor of the <see cref="CoreHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">An <see cref="HttpClient"/> that will execute the request</param>
        public CoreHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Set the timeout for the next request.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void SetTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
        }

        /// <summary>
        /// Sends an <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="httpRequestMessage">The <see cref="HttpResponseMessage"/> to be sent.</param>
        /// <returns>An <see cref="HttpResponseMessage"/>.</returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            if (Timeout != TimeSpan.Zero)
                _httpClient.Timeout = Timeout;
            return await _httpClient.SendAsync(httpRequestMessage);
        }

    }
}