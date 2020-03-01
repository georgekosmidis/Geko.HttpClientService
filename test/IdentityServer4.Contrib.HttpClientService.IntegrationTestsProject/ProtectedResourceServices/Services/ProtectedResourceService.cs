using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.FeaturesSample.ProtectedResourceServices.Dto;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.FeaturesSample.ProtectedResourceServices.Services
{
    /// <summary>
    /// A test service for the protected resource https://demo.identityserver.io/api/test
    /// </summary>
    public class ProtectedResourceService
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;
        private readonly IOptions<ProtectedResourceAccessTokenOptions> _identityServerOptions;

        /// <summary>
        /// Constructor for the <see cref="ProtectedResourceService"/>.
        /// </summary>
        /// <param name="requestServiceFactory">The <see cref="IHttpClientServiceFactory"/> implementation that will perform the request to the protected resource.</param>
        /// <param name="identityServerOptions">The identity server options that will used to retrieve an access token.</param>
        public ProtectedResourceService(IHttpClientServiceFactory requestServiceFactory, IOptions<ProtectedResourceAccessTokenOptions> identityServerOptions)
        {
            _requestServiceFactory = requestServiceFactory;
            _identityServerOptions = identityServerOptions;
        }

        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TestApiResponseDto}"/>. </returns>
        public async Task<IEnumerable<ProtectedResourceResponseDto>> GetTestApiResults()
        {
            var response = await _requestServiceFactory
                .CreateHttpClientService(nameof(ProtectedResourceService))                                                //Try to always use named HttpClients, read more here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .SetIdentityServerOptions(_identityServerOptions)                                                            //Set the options to retrieve an access token
                .GetAsync<IEnumerable<ProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test");          //Execute the request

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.BodyAsType;
            else
                throw new SystemException(response.StatusCode.ToString());
        }

        /// <summary>
        /// Sample request that returns the entire <see cref="ResponseObject{TResponseBody}"/> object using GET
        /// </summary>
        /// <param name="headers">A <see cref="Dictionary{TKey, TValue}"/> with the key representing the name of the header, and the value representing the value of the header.</param>
        /// <returns>The entire <see cref="ResponseObject{TResponseBody}"/> object produced.</returns>
        public async Task<ResponseObject<IEnumerable<ProtectedResourceResponseDto>>> GetTestResponseObject(Dictionary<string, string> headers)
        {
            var response = await _requestServiceFactory
                .CreateHttpClientService(nameof(ProtectedResourceService))                                       //Try to always use named HttpClients, read more here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .SetIdentityServerOptions(_identityServerOptions)                                                   //Set the options to retrieve an access token
                .SetHeaders(headers)                                                                             //Set custom headers
                .GetAsync<IEnumerable<ProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test"); //Execute the request

            return response;
        }
    }
}
