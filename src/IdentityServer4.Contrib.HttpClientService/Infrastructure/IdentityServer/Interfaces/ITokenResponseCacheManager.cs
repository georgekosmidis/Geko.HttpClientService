using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Interface for the cache manager for the access token service. 
    /// </summary>
    /// <remarks>
    /// It caches a successful response for the 75% of the expiration time define in the <see cref="TokenResponse"/>. 
    /// </remarks>
    public interface ITokenResponseCacheManager
    {
        /// <summary>
        /// Adds a new <see cref="TokenResponse"/> in the cache by executing the <paramref name="call"/> parameter, or returns an existing cached response.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="call">The delegate the returns a <see cref="TokenResponse"/>.</param>
        /// <returns>A <see cref="TokenResponse"/>, either just aqcuired or from the cache</returns>
        Task<TokenResponse> AddOrGetExistingAsync(string key, Func<Task<TokenResponse>> call);

        /// <summary>
        /// Removes an entry from the cache
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        void Remove(string key);
    }
}