using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices;
using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample
{
    /// <summary>
    /// The Startup class configures services and the app's request pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor for the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services">A collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            /********************************************************************************************/
            //Adds the HTTP client service factory to the service collection,
            // ads as singleton a custom service for HTTP calls (ProtectedResourceService),
            // and registers a configuration instance with the client credentials options that will be used the ProtectedResourceService
            services.AddHttpClientService()
                    .AddSingleton<IProtectedResourceService, ProtectedResourceService>()
                    .Configure<ProtectedResourceClientCredentialsOptions>(Configuration.GetSection(nameof(ProtectedResourceClientCredentialsOptions)));
            /********************************************************************************************/

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app">Used to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
