using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using IdentityServer4.HttpClientService.Extensions;
using IdentityServer4.HttpClientService.Models;

namespace IdentityServer4.HttpClientService.Infrastructure
{

    /// <summary>
    /// The implementation of an access token service for client credentials based on IdentityServer4. 
    /// </summary>
    public class TokenResponseService : ITokenResponseService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenResponseCacheManager _cache;

        /// <summary>
        /// Constructor of the <see cref="TokenResponseService" />
        /// </summary>
        /// <param name="httpClientFactory">An <see cref="HttpClientFactory"/> to create an <see cref="HttpClient"/>.</param>
        /// <param name="cache">A cache manager instance to cache handle the caching of the Identity Server response.</param>
        public TokenResponseService(IHttpClientFactory httpClientFactory, ITokenResponseCacheManager cache)
        {
            _httpClient = httpClientFactory.CreateClient();
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
                    return await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = options.Value.Address,
                        ClientId = options.Value.ClientId,
                        ClientSecret = options.Value.ClientSecret,
                        Scope = options.Value.Scopes
                    });
                }
             );

            return tokenResponse;
        }
    }
}