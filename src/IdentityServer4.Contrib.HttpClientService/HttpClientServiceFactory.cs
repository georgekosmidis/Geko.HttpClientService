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
        private readonly ICoreHttpClient _coreHttpClient;
        private readonly ITokenResponseService _tokenResponseService;
        private readonly IHttpRequestMessageFactory _requestMessageFactory;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor of the <see cref="HttpClientServiceFactory" />.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="coreHttpClient">An <see cref="ICoreHttpClient"/> implementation that will execute the HTTP requests.</param>
        /// <param name="requestMessageFactory">The <see cref="IHttpRequestMessageFactory"/> to get a new <see cref="HttpRequestMessage"/>.</param>
        /// <param name="tokenResponseService">The <see cref="ITokenResponseService"/> to retrieve a token, if required.</param>
        public HttpClientServiceFactory(IConfiguration configuration, ICoreHttpClient coreHttpClient, IHttpRequestMessageFactory requestMessageFactory, ITokenResponseService tokenResponseService)
        {
            _configuration = configuration;
            _coreHttpClient = coreHttpClient;
            _tokenResponseService = tokenResponseService;
            _requestMessageFactory = requestMessageFactory;
        }

        /// <summary>
        /// Creates new <see cref="HttpClientService"/> instances. 
        /// </summary>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        public HttpClientService CreateHttpClientService()
        {
            return new HttpClientService(_configuration, _coreHttpClient, _requestMessageFactory, _tokenResponseService);
        }
    }
}