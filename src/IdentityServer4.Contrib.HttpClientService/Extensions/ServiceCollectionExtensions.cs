using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using System;

namespace IdentityServer4.Contrib.HttpClientService.Extensions
{
    /// <summary>
    /// Extensions methods for setting up the RequestService in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the RequestService to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services.</returns>
        public static IServiceCollection AddHttpClientService(this IServiceCollection services)
        {
            services.AddHttpClient<IIdentityServerHttpClient, ClientCredentialsHttpClient>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddHttpClient<IIdentityServerHttpClient, PasswordHttpClient>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddSingleton<IIdentityServerHttpClientSelector, IdentityServerHttpClientSelector>();

            services.AddSingleton<IIdentityServerService, IdentityServerService>();
            services.AddMemoryCache();
            services.AddSingleton<ITokenResponseCacheManager, TokenResponseCacheManager>();

            services.AddHttpClient<ICoreHttpClient, CoreHttpClient>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpRequestMessageFactory, HttpRequestMessageFactory>();
            services.AddSingleton<IHttpClientServiceFactory, HttpClientServiceFactory>();

            return services;
        }
        
    }


}