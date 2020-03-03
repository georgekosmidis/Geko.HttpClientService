using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Dto;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services
{
    /// <summary>
    /// A test service for the protected resource https://demo.identityserver.io/api/test
    /// </summary>

    public class ProtectedResourceService : IProtectedResourceService
    {
        private readonly ILogger<ProtectedResourceService> _logger;
        private readonly IHttpClientServiceFactory _requestServiceFactory;
        private readonly IOptions<ProtectedResourceClientCredentialsOptions> _identityServerOptions;

        /// <summary>
        /// Constructor for the <see cref="TestApiService"/>.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="requestServiceFactory">The <see cref="IHttpClientServiceFactory"/> that will perform the request to the protected resource.</param>
        /// <param name="identityServerOptions">The identity server options that will used to retrieve an access token.</param>
        public ProtectedResourceService(ILogger<ProtectedResourceService> logger, IHttpClientServiceFactory requestServiceFactory, IOptions<ProtectedResourceClientCredentialsOptions> identityServerOptions)
        {
            _logger = logger;
            _requestServiceFactory = requestServiceFactory;
            _identityServerOptions = identityServerOptions;
        }

        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ProtectedResourceResponseDto}"/>. </returns>
        public async Task<IEnumerable<ProtectedResourceResponseDto>> GetProtectedResourceResults()
        {
            var response = await _requestServiceFactory
                .CreateHttpClientService()
                .SetIdentityServerOptions(_identityServerOptions)                                                   //Set the options to retrieve an access token
                .AddHeader("X-Request-Client", "IdentityServer4.Contrib.HttpClientService")                         //Set custom headers for this request
                .GetAsync<IEnumerable<ProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test");    //Execute the request

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.BodyAsType;
            }
            else
            {
                //log, error handle, etc
                _logger.LogError(response.StatusCode + " " + response.Error);
                return new List<ProtectedResourceResponseDto>();
            }
        }


    }
}
