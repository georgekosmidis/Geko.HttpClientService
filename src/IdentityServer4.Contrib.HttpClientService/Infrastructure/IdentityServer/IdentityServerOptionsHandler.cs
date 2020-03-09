using IdentityServer4.Contrib.HttpClientService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// An object that handles the IdentityServer4 client credentials registration
    /// </summary>
    public class IdentityServerOptionsHandler
    {
        private readonly IConfiguration _configuration;
        private ClientCredentialOptions defaultClientCredentialOptions;

        /// <summary>
        /// Constructor for the <see cref="IdentityServerOptionsHandler"/>
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        public IdentityServerOptionsHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets the <see cref="ClientCredentialOptions"/> set for this instance
        /// </summary>
        /// <returns>The <see cref="ClientCredentialOptions"/> of this instance</returns>
        public ClientCredentialOptions Get()
        {
            return defaultClientCredentialOptions;
        }

        /// <summary>
        /// Checks if client credentials options have been set
        /// </summary>
        public bool HasOptions
        {
            get
            {
                return defaultClientCredentialOptions != null;
            }
        }
        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token using client credentials by passing the appsettings configuration section 
        /// that contain the necessary configuration keys.
        /// </summary>
        /// <param name="configurationSection">
        /// The name of the configuration section that contains the information for requesting an access token. 
        /// See <see cref="ClientCredentialOptions"/> for the property names.
        /// </param>
        public void Set(string configurationSection)
        {
            if (_configuration == null)
                throw new InvalidOperationException("The string configuraton cannot be used with the the lazy singleton instance of " + nameof(HttpClientService) + " (" + nameof(HttpClientServiceFactory) + "." + nameof(HttpClientServiceFactory.Instance) + ")");

            var sectionExists = _configuration.GetChildren().Any(item => item.Key == configurationSection);
            if (!sectionExists)
                throw new ArgumentException("The configuration section '" + configurationSection + "' cannot be found!", nameof(configurationSection));

#warning Select correct options based on configuration section contents
            var options = _configuration.GetSection(configurationSection).Get<ClientCredentialOptions>();

            Set(options);

        }

        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token using client credentials.
        /// </summary>
        /// <typeparam name="TOptions">The type of the <paramref name="options"/>.</typeparam>
        /// <param name="options">The token service options.</param>
        public void Set<TOptions>(IOptions<TOptions> options)
            where TOptions : class, IIdentityServerOptionsConstrain, new()
        {
            Set(options.Value);
        }

        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token using client credentials by using a delegate.
        /// </summary>
        /// <typeparam name="TOptions">The type of the <paramref name="optionsDelegate"/>.</typeparam>
        /// <param name="optionsDelegate">The <see cref="Action{T}"/> delegate.</param>
        public void Set<TOptions>(Action<TOptions> optionsDelegate) 
            where TOptions : class, IIdentityServerOptionsConstrain
        {
            var obj = new ClientCredentialOptions();
            optionsDelegate(obj as TOptions);

            Set(obj);
        }

        /// <summary>
        /// Sets the IdentityServer4 options by passing an object that inherits from <see cref="IIdentityServerOptionsConstrain"/>
        /// </summary>
        /// <param name="options">The <see cref="IIdentityServerOptionsConstrain"/> that contains the options.</param>
        public void Set<TOptions>(TOptions options) 
            where TOptions : IIdentityServerOptionsConstrain
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Address == null)
            {
                throw new ArgumentNullException(nameof(options.Address));
            }

            if (options.ClientId == null)
            {
                throw new ArgumentNullException(nameof(options.ClientId));
            }

            if (options.ClientSecret == null)
            {
                throw new ArgumentNullException(nameof(options.ClientSecret));
            }

            defaultClientCredentialOptions = new ClientCredentialOptions
            {
                Address = options.Address,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                Scope = options.Scope
            };
        }

    }
}
