using IdentityModel.Client;
using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Options;
using System;
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
        /// The type of the <see cref="IIdentityServerOptions"/> implementation
        /// </summary>
        public Type HttpClientOptionsType { get; }

        /// <summary>
        /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
        /// </summary>
        /// <param name="options">The token service options</param>
        /// <returns>Returns a string representing a unique identifier to be used as the caching key.</returns>
        string GetCacheKey(IIdentityServerOptions options);

        /// <summary>
        /// Retrieves a <see cref="TokenResponse"/> from the configured by the <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The configuration options for the IdentityServer4.</param>
        /// <returns>A <see cref="TokenResponse"/> object.</returns>
        Task<TokenResponse> GetTokenResponseAsync(IIdentityServerOptions options);
    }
}