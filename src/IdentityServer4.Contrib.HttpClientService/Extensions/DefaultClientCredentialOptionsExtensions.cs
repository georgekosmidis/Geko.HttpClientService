using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.Extensions
{
    /// <summary>
    /// Static object for <see cref="IOptions{TTokenServiceOptions}" /> extensions.
    /// </summary>
    internal static class DefaultClientCredentialOptionsExtensions
    {
        /// <summary>
        /// Creates a unique key to be used as the cache key of the Identity Server access token, by combining infomration from the access token options object.
        /// See <see cref="DefaultClientCredentialOptions"/> for the access token options.
        /// </summary>
        /// <typeparam name="TTokenServiceOptions">A type that inherits from the <see cref="DefaultClientCredentialOptions"/> onject</typeparam>
        /// <param name="options">The token service options</param>
        /// <returns>Returns a string representing a unique identifier to be used as the caching key, by getting the hashcode of the address, the client and scopes.</returns>
        internal static string GetCacheKey<TTokenServiceOptions>(this IOptions<TTokenServiceOptions> options) where TTokenServiceOptions : DefaultClientCredentialOptions, new()
        {
            return (options.Value.Address + options.Value.ClientId + options.Value.Scopes).GetHashCode().ToString();
        }
    }
}