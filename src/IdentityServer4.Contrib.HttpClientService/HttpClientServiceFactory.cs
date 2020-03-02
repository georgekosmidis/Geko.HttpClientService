using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Models;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4.Contrib.HttpClientService
{
    /// <summary>
    /// A factory that creates <see cref="HttpClientService"/> instances for a given logical name.
    /// </summary>
    public class HttpClientServiceFactory : IHttpClientServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenResponseService _accessTokenService;
        private readonly IHttpRequestMessageFactory _requestMessageFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor of the <see cref="HttpClientServiceFactory" />.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> to create an <see cref="HttpClient"/>.</param>
        /// <param name="requestMessageFactory">The <see cref="IHttpRequestMessageFactory"/> to get a new <see cref="HttpRequestMessage"/>.</param>
        /// <param name="accessTokenService">The <see cref="ITokenResponseService"/> to retrieve a token, if required.</param>
        public HttpClientServiceFactory(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpRequestMessageFactory requestMessageFactory, ITokenResponseService accessTokenService)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _accessTokenService = accessTokenService;
            _requestMessageFactory = requestMessageFactory;
        }

        /// <summary>
        /// Constructor of the <see cref="HttpClientServiceFactory" /> without the<see cref= "IConfiguration" /> dependency.
        /// The <see cref="HttpClientService.SetIdentityServerOptions(string)" /> will throw an <see cref="InvalidOperationException" /> with this constructor,
        /// please use the <see cref="HttpClientService.SetIdentityServerOptions{TTokenServiceOptions}(IOptions{TTokenServiceOptions})"/>.
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="requestMessageFactory"></param>
        /// <param name="accessTokenService"></param>
        public HttpClientServiceFactory(IHttpClientFactory httpClientFactory, IHttpRequestMessageFactory requestMessageFactory, ITokenResponseService accessTokenService)
        {
            _httpClientFactory = httpClientFactory;
            _accessTokenService = accessTokenService;
            _requestMessageFactory = requestMessageFactory;
        }

        /// <summary>
        /// Creates new <see cref="HttpClientService"/> instances. 
        /// Prefer <see cref="CreateHttpClientService(string)"/> when possible, to avoid 'sockets exhaustion' issues caused by HttpClient.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </summary>
        /// <remarks>
        /// Prefer <see cref="CreateHttpClientService(string)"/> when possible, to avoid 'sockets exhaustion'.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </remarks>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        public HttpClientService CreateHttpClientService()
        {
            return new HttpClientService(_configuration, _httpClientFactory, _requestMessageFactory, _accessTokenService)
                .CreateHttpClient();
        }

        /// <summary>
        /// Creates or returns <see cref="HttpClientService"/> instances for the given logical name.
        /// </summary>
        /// <remarks>
        /// By specifing a name, the HTTPClient will be reused to avoid 'sockets exhaustion' issues caused by HttpClient.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </remarks>
        /// <param name="name">The name of the <see cref="HttpClient"/> that will be used.</param>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        public HttpClientService CreateHttpClientService(string name)
        {

            return new HttpClientService(_configuration, _httpClientFactory, _requestMessageFactory, _accessTokenService)
                .CreateHttpClient(name);
        }
    }
}