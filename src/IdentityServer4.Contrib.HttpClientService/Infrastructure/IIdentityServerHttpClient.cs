using IdentityModel.Client;
using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Abstraction for a typed <see cref="HttpClient"/> that will execute the request to the IdentityServer4.
    /// </summary>
    public interface IIdentityServerHttpClient
    {
        /// <summary>
        /// Retrieves a <see cref="TokenResponse"/> from the configured by the <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TTokenServiceOptions">The type of the configuration options for the IdentityServer4.</typeparam>
        /// <param name="options">The configuration options for the IdentityServer4.</param>
        /// <returns>A <see cref="TokenResponse"/> object.</returns>
        Task<TokenResponse> GetTokenResponseAsync<TTokenServiceOptions>(TTokenServiceOptions options) where TTokenServiceOptions : DefaultClientCredentialOptions, new();
    }
}