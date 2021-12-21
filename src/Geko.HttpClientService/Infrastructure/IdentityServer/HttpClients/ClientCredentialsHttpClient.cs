using Geko.HttpClientService.Models;
using IdentityModel.Client;

namespace Geko.HttpClientService.Infrastructure;

/// <summary>
/// The implementation of an access token service for client credentials based on IdentityServer4. 
/// </summary>
public class ClientCredentialsHttpClient : IIdentityServerHttpClient
{

    /// <summary>
    /// The type of the <see cref="IIdentityServerOptions"/> implementation that this HttpClient needs
    /// </summary>
    public Type HttpClientOptionsType => typeof(ClientCredentialsOptions);
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor of the <see cref="ClientCredentialsHttpClient"/>.
    /// </summary>
    /// <param name="httpClient">An <see cref="HttpClient"/> that will execute the request</param>
    public ClientCredentialsHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
    /// See <see cref="ClientCredentialsOptions"/> for the access token options.
    /// </summary>
    /// <param name="options">The token service options</param>
    /// <returns>Returns a string representing a unique identifier to be used as the caching key, by getting the hashcode of the address, the client and scopes.</returns>
    public string GetCacheKey(IIdentityServerOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var clientCredentialOptions = options as ClientCredentialsOptions;

        if (clientCredentialOptions == null)
        {
            throw new InvalidOperationException("The '" + nameof(options) + "' argument cannot be expicitly converted to " + nameof(ClientCredentialsOptions) + ".");
        }

        if (clientCredentialOptions.Address == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.Address) + " cannot be null.");
        }

        if (clientCredentialOptions.ClientId == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.ClientId) + " cannot be null.");
        }

        if (clientCredentialOptions.Scope == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.Scope) + " cannot be null.");
        }

        return (clientCredentialOptions.Address + clientCredentialOptions.ClientId + clientCredentialOptions.Scope).GetHashCode().ToString();
    }

    /// <summary>
    /// Retrieves a <see cref="TokenResponse"/> from the configured by the <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The <see cref="ClientCredentialsOptions"/> for the IdentityServer4.</param>
    /// <returns>A <see cref="TokenResponse"/> object.</returns>
    public async Task<TokenResponse> GetTokenResponseAsync(IIdentityServerOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var clientCredentialOptions = options as ClientCredentialsOptions;

        if (clientCredentialOptions == null)
        {
            throw new InvalidOperationException("The '" + nameof(options) + "' argument cannot be expicitly converted to " + nameof(ClientCredentialsOptions) + ".");
        }

        if (clientCredentialOptions.Address == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.Address) + " cannot be null.");
        }

        if (clientCredentialOptions.ClientId == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.ClientId) + " cannot be null.");
        }

        if (clientCredentialOptions.Scope == null)
        {
            throw new InvalidOperationException(nameof(clientCredentialOptions.Scope) + " cannot be null.");
        }

        return await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = clientCredentialOptions.Address,
            ClientId = clientCredentialOptions.ClientId,
            ClientSecret = clientCredentialOptions.ClientSecret,
            Scope = clientCredentialOptions.Scope
        });
    }
}
