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
    public class TokenResponseService : ITokenResponseService
    {
        private readonly IIdentityServerHttpClient _identityServerHttpClient;
        private readonly ITokenResponseCacheManager _cache;

        /// <summary>
        /// Constructor of the <see cref="TokenResponseService"/>.
        /// </summary>
        /// <param name="identityServerHttpClient">A typed <see cref="HttpClient"/> for the token service.</param>
        /// <param name="cache">A cache engine imlementation, to cache the token response.</param>
        public TokenResponseService(IIdentityServerHttpClient identityServerHttpClient, ITokenResponseCacheManager cache)
        {
            _identityServerHttpClient = identityServerHttpClient;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves either a new access token using client credentials or the last valid from the caching engine.
        /// </summary>
        /// <typeparam name="TTokenServiceOptions">A type that inherits from the <see cref="DefaultClientCredentialOptions"/> onject</typeparam>
        /// <param name="options">The token service options</param>
        /// <returns>A <see cref="TokenResponse"/> instance.</returns>
        public async Task<TokenResponse> GetTokenResponseAsync<TTokenServiceOptions>(IOptions<TTokenServiceOptions> options) where TTokenServiceOptions : DefaultClientCredentialOptions, new()
        {
            if (options == default)
                throw new ArgumentNullException(nameof(options));

            var tokenResponse = await _cache.AddOrGetExistingAsync(
                options.GetCacheKey(),
                async () =>
                {
                    return await _identityServerHttpClient.GetTokenResponseAsync(options);
                }
             );

            return tokenResponse;
        }
    }
}