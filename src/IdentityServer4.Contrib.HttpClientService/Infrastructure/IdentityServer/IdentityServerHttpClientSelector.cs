using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using IdentityServer4.Contrib.HttpClientService.Models;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{

    /// <summary>
    /// Selects the correct <see cref="IIdentityServerHttpClient"/> based on <see cref="IIdentityServerOptions"/>.
    /// </summary>
    public class IdentityServerHttpClientSelector : IIdentityServerHttpClientSelector
    {
        private IEnumerable<IIdentityServerHttpClient> _httpClients;

        /// <summary>
        /// Constructor of the <see cref="IdentityServerHttpClientSelector"/>.
        /// </summary>
        public IdentityServerHttpClientSelector(IEnumerable<IIdentityServerHttpClient> httpClients)
        {
            _httpClients = httpClients;
        }

        /// <summary>
        /// Finds the appropriate implementation of <see cref="IIdentityServerHttpClient"/> based on the <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <paramref name="options"/> for retrieving an access token.</param>
        /// <returns>An <see cref="IIdentityServerHttpClient"/>.</returns>
        public IIdentityServerHttpClient Get(IIdentityServerOptions options)
        {
            if (!_httpClients.Any(x => x.HttpClientOptionsType.IsAssignableFrom(options.GetType())))
            {
                throw new InvalidOperationException("There is no assignable type for the options selected.");
            }

            return _httpClients.First(x => x.HttpClientOptionsType.IsAssignableFrom(options.GetType()));
        }

    }
}