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
    public class PasswordHttpClient : IIdentityServerHttpClient
    {
        /// <summary>
        /// The type of the <see cref="IIdentityServerOptions"/> implementation that this HttpClient needs
        /// </summary>
        public Type HttpClientOptionsType => typeof(PasswordOptions);

        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor of the <see cref="ClientCredentialsHttpClient"/>.
        /// </summary>
        /// <param name="httpClient">An <see cref="HttpClient"/> that will execute the request</param>
        public PasswordHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
        /// See <see cref="PasswordOptions"/> for the access token options.
        /// </summary>
        /// <param name="options">The token service options</param>
        /// <returns>Returns a string representing a unique identifier to be used as the caching key, by getting the hashcode of the address, the client, the username and the scopes.</returns>
        public string GetCacheKey(IIdentityServerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var passwordOptions = options as PasswordOptions;

            if (passwordOptions == null)
            {
                throw new InvalidOperationException("The '" + nameof(options) + "' argument cannot be expicitly converted to " + nameof(PasswordOptions) + ".");
            }

            if (passwordOptions.Address == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Address) + " cannot be null.");
            }

            if (passwordOptions.ClientId == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.ClientId) + " cannot be null.");
            }

            if (passwordOptions.Scope == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Scope) + " cannot be null.");
            }

            if (passwordOptions.Username == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Username) + " cannot be null.");
            }

            return (passwordOptions.Address + passwordOptions.ClientId + passwordOptions.Scope + passwordOptions.Username).GetHashCode().ToString();
        }

        /// <summary>
        /// Retrieves a <see cref="TokenResponse"/> from the configured by the <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="PasswordOptions"/> for the IdentityServer4.</param>
        /// <returns>A <see cref="TokenResponse"/> object.</returns>
        public async Task<TokenResponse> GetTokenResponseAsync(IIdentityServerOptions options)
        {
            if (options == default)
                throw new ArgumentNullException(nameof(options));

            var passwordOptions = options as PasswordOptions;

            if (passwordOptions == null)
            {
                throw new InvalidOperationException("The '" + nameof(options) + "' argument cannot be expicitly converted to " + nameof(PasswordOptions) + ".");
            }

            if (passwordOptions.Address == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Address) + " cannot be null.");
            }

            if (passwordOptions.ClientId == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.ClientId) + " cannot be null.");
            }

            if (passwordOptions.Scope == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Scope) + " cannot be null.");
            }

            if (passwordOptions.Username == null)
            {
                throw new InvalidOperationException(nameof(passwordOptions.Username) + " cannot be null.");
            }

            return await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = passwordOptions.Address,
                ClientId = passwordOptions.ClientId,
                ClientSecret = passwordOptions.ClientSecret,
                Scope = passwordOptions.Scope,
                UserName = passwordOptions.Username,
                Password = passwordOptions.Password
            });
        }
    }
}