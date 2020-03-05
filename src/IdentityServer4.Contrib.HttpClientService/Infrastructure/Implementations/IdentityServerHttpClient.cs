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
    public class IdentityServerHttpClient : IIdentityServerHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor of the <see cref="IdentityServerHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">An <see cref="HttpClient"/> that will execute the request</param>
        public IdentityServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a <see cref="TokenResponse"/> from the configured by the <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TTokenServiceOptions">The type of the configuration options for the IdentityServer4.</typeparam>
        /// <param name="options">The configuration options for the IdentityServer4.</param>
        /// <returns>A <see cref="TokenResponse"/> object.</returns>
        public async Task<TokenResponse> GetTokenResponseAsync<TTokenServiceOptions>(TTokenServiceOptions options) where TTokenServiceOptions : DefaultClientCredentialOptions, new()
        {
            if (options == default)
                throw new ArgumentNullException(nameof(options));

            return await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = options.Address,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                Scope = options.Scope
            });
        }
    }
}