using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.Extensions
{
    /// <summary>
    /// Static object extensions for IdentityServer4 options.
    /// </summary>
    internal static class IdentityServer4OptionsExtensions
    {
        /// <summary>
        /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
        /// See <see cref="ClientCredentialOptions"/> for the access token options.
        /// </summary>
        /// <param name="options">The token service options</param>
        /// <returns>Returns a string representing a unique identifier to be used as the caching key, by getting the hashcode of the address, the client and scopes.</returns>
        internal static string GetCacheKey(this ClientCredentialOptions options)
        {
            return (options.Address + options.ClientId + options.Scope).GetHashCode().ToString();
        }

        /// <summary>
        /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
        /// See <see cref="ClientCredentialOptions"/> for the access token options.
        /// </summary>
        /// <param name="options">The token service options</param>
        /// <returns>Returns a string representing a unique identifier to be used as the caching key, by getting the hashcode of the address, the client and scopes.</returns>
        internal static string GetCacheKey(this PasswordOptions options)
        {
            return (options.Address + options.ClientId + options.Scope + options.Username).GetHashCode().ToString();
        }
    }
}