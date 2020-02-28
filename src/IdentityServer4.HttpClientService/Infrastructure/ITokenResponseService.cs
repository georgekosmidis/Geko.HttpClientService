using IdentityModel.Client;
using IdentityServer4.HttpClientService.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace IdentityServer4.HttpClientService.Infrastructure
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
        Task<TokenResponse> GetTokenResponseAsync<TTokenServiceOptions>(IOptions<TTokenServiceOptions> options) where TTokenServiceOptions : DefaultClientCredentialOptions, new();
    }
}