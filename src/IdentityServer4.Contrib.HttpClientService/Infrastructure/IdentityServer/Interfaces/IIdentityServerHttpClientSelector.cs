using IdentityServer4.Contrib.HttpClientService.Models;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// Abstraction for an <see cref="IIdentityServerHttpClient"/> selector. 
    /// The correct client is selected based on <see cref="IIdentityServerOptions"/>.
    /// </summary>
    public interface IIdentityServerHttpClientSelector
    {
        /// <summary>
        /// Finds the appropriate implementation of <see cref="IIdentityServerHttpClient"/> based on the <paramref name="options"/>
        /// </summary>
        /// <param name="options">The <paramref name="options"/> for retrieving an access token</param>
        /// <returns>An <see cref="IIdentityServerHttpClient"/></returns>
        IIdentityServerHttpClient Get(IIdentityServerOptions options);
    }
}