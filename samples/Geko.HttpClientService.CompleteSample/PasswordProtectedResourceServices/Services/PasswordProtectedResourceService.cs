using Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Dto;
using Geko.HttpClientService.Extensions;
using Microsoft.Extensions.Options;

namespace Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services;

/// <summary>
/// A test service for the protected resource https://demo.identityserver.io/api/test
/// </summary>

public class PasswordProtectedResourceService : IPasswordProtectedResourceService
{
    private readonly ILogger<PasswordProtectedResourceService> _logger;
    private readonly HttpClientService _requestService;
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
        _requestService = requestServiceFactory.CreateHttpClientService();
        _identityServerOptions = identityServerOptions;
    }

    /// <summary>
    /// Returns true if an access token is succesfully retrieved
    /// </summary>
    /// <param name="username">The username</param>
    /// <param name="password">The username</param>
    /// <returns>A boolean indicating if the access token has benn succesfully retrieved.</returns>
    public async Task<bool> TrySettingPasswordOptions(string username, string password)
    {
        var response = await _requestService
            .SetIdentityServerOptions<SomePasswordOptions>(x =>
            {
                x.Address = _identityServerOptions.Value.Address;
                x.ClientId = _identityServerOptions.Value.ClientId;
                x.ClientSecret = _identityServerOptions.Value.ClientSecret;
                x.Scope = _identityServerOptions.Value.Scope;
                x.Username = username;
                x.Password = password;
            })
            .GetTokenResponse();

        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    /// <summary>
    /// Sample request that returns a typed response using GET
    /// </summary>
    /// <returns>An <see cref="IEnumerable{ProtectedResourceResponseDto}"/>. </returns>
    public async Task<IEnumerable<PasswordProtectedResourceResponseDto>> GetProtectedResourceResults()
    {
        var response = await _requestService
            .HeadersAdd("X-Request-Client", "Geko.HttpClientService")                        //Set custom headers for this request
            .GetAsync<IEnumerable<PasswordProtectedResourceResponseDto>>("https://demo.identityserver.io/api/test");    //Execute the request

        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? response.BodyAsType ?? new List<PasswordProtectedResourceResponseDto>()
            : throw new InvalidOperationException("No PasswordOptions set. Please use 'TrySettingPasswordOptions(string, string)' before calling this method");
    }


}
