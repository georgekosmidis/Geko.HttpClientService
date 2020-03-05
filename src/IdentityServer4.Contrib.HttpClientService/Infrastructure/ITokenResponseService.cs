using IdentityModel.Client;
using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Abstraction for an access token service.
    /// </summary>
    public interface ITokenResponseService
    {
        /// <summary>
        /// Retrieves a <see cref="TokenResponse"/>  from the configured Identity Server
        /// </summary>
        /// <typeparam name="TTokenServiceOptions">A type that inherits from the <see cref="DefaultClientCredentialOptions"/> onject</typeparam>
        /// <param name="options">The token service options</param>
        /// <returns>A <see cref="TokenResponse"/> instance.</returns>
        Task<TokenResponse> GetTokenResponseAsync<TTokenServiceOptions>(TTokenServiceOptions options) where TTokenServiceOptions : DefaultClientCredentialOptions, new();
    }
}