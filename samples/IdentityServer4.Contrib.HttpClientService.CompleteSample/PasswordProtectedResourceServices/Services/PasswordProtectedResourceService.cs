using IdentityServer4.Contrib.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Dto;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services
{
    /// <summary>
    /// A test service for the protected resource https://demo.identityserver.io/api/test
    /// </summary>

    public class PasswordProtectedResourceService : IPasswordProtectedResourceService
    {
        private readonly ILogger<PasswordProtectedResourceService> _logger;
        private readonly IHttpClientServiceFactory _requestServiceFactory;
        private readonly IOptions<SomePasswordOptions> _identityServerOptions;

        /// <summary>
        /// Constructor for the <see cref="TestApiService"/>.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="requestServiceFactory">The <see cref="IHttpClientServiceFactory"/> that will perform the request to the protected resource.</param>
        /// <param name="identityServerOptions">The identity server options that will used to retrieve an access token.</param>
        public PasswordProtectedResourceService(ILogger<PasswordProtectedResourceService> logger, IHttpClientServiceFactory requestServiceFactory, IOptions<SomePasswordOptions> identityServerOptions)
        {
            _logger = logger;
            _requestServiceFactory = requestServiceFactory;
            _identityServerOptions = identityServerOptions;
        }

        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ProtectedResourceResponseDto}"/>. </returns>
        public async Task<IEnumerable<PasswordProtectedResourceResponseDto>> GetProtectedResourceResults(string username, string password)
        {
            
            var response = await _requestServiceFactory
                .CreateHttpClientService()
                .SetIdentityServerOptions<SomePasswordOptions>(x =>                                                 //Set the options to retrieve an access token
                {
                    x.Address = _identityServerOptions.Value.Address;
                    x.ClientId = _identityServerOptions.Value.ClientId;
                    x.ClientSecret = _identityServerOptions.Value.ClientSecret;
                    x.Scope = _identityServerOptions.Value.Scope;
                    x.Username = username;
                    x.Password = password;
                })                                                                                                  
                .HeadersAdd("X-Request-Client", "IdentityServer4.Contrib.HttpClientService")                        //Set custom headers for this request
                .GetAsync<IEnumerable<PasswordProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test");    //Execute the request

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.BodyAsType;
            }
            else
            {
                //log, error handle, etc
                _logger.LogError(response.StatusCode + " " + response.Error);
                return new List<PasswordProtectedResourceResponseDto>();
            }
        }


    }
}
