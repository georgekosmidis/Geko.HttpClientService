using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{

    /// <summary>
    /// The implementation of an access token service for client credentials based on IdentityServer4. 
    /// </summary>
    public class IdentityServerService : IIdentityServerService
    {
        private readonly IIdentityServerHttpClientSelector _identityServerHttpClientSelector;
        private readonly ITokenResponseCacheManager _cache;

        /// <summary>
        /// Constructor of the <see cref="IdentityServerService"/>.
        /// </summary>
        /// <param name="identityServerHttpClientSelector">An Identity Server HTTP client selected that will fetch the correct HTTP client.</param>
        /// <param name="cache">A cache engine imlementation, to cache the token response.</param>
        public IdentityServerService(IIdentityServerHttpClientSelector identityServerHttpClientSelector, ITokenResponseCacheManager cache)
        {
            _identityServerHttpClientSelector = identityServerHttpClientSelector;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves either a new access token using client credentials or the last valid from the caching engine.
        /// </summary>
        /// <param name="options">The token service options.</param>
        /// <returns>A <see cref="TokenResponse"/> instance.</returns>
        public async Task<TokenResponse> GetTokenResponseAsync(IIdentityServerOptions options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var httpClient = _identityServerHttpClientSelector.Get(options);

            var tokenResponse = await _cache.AddOrGetExistingAsync(
                httpClient.GetCacheKey(options),
                async () =>
                {
                    return await httpClient.GetTokenResponseAsync(options);
                }
             );

            return tokenResponse;
        }

    }
}