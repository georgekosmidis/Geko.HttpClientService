using Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Dto;
using Geko.HttpClientService.Extensions;
using Microsoft.Extensions.Options;

namespace Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Services;

/// <summary>
/// A test service for the protected resource https://demo.identityserver.io/api/test
/// </summary>
public class ClientCredentials2ProtectedResourceService : IClientCredentials2ProtectedResourceService
{
    private readonly ILogger<ClientCredentials2ProtectedResourceService> _logger;
    private readonly IHttpClientServiceFactory _requestServiceFactory;
    private readonly IOptions<OtherClientCredentialsOptions> _identityServerOptions;

    /// <summary>
    /// Constructor for the <see cref="TestApiService"/>.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="requestServiceFactory">The <see cref="IHttpClientServiceFactory"/> that will perform the request to the protected resource.</param>
    /// <param name="identityServerOptions">The identity server options that will used to retrieve an access token.</param>
    public ClientCredentials2ProtectedResourceService(ILogger<ClientCredentials2ProtectedResourceService> logger, IHttpClientServiceFactory requestServiceFactory, IOptions<OtherClientCredentialsOptions> identityServerOptions)
    {
        _logger = logger;
        _requestServiceFactory = requestServiceFactory;
        _identityServerOptions = identityServerOptions;
    }

    /// <summary>
    /// Sample request that returns a typed response using GET
    /// </summary>
    /// <returns>An <see cref="IEnumerable{ProtectedResourceResponseDto}"/>. </returns>
    public async Task<IEnumerable<ClientCredentials2ProtectedResourceResponseDto>> GetProtectedResourceResults()
    {
        var response = await _requestServiceFactory
            .CreateHttpClientService()
            .SetIdentityServerOptions(_identityServerOptions)                                                   //Set the options to retrieve an access token
            .HeadersAdd("X-Request-Client", "Geko.HttpClientService")                        //Set custom headers for this request
            .GetAsync<IEnumerable<ClientCredentials2ProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test");    //Execute the request

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return response.BodyAsType ?? new List<ClientCredentials2ProtectedResourceResponseDto>();
        }
        else
        {
            //log, error handle, etc
            _logger.LogError(response.StatusCode + " " + response.Error);
            return new List<ClientCredentials2ProtectedResourceResponseDto>();
        }
    }


}
