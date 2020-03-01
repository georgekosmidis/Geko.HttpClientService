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
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Adds the HTTP client service factory to the service collection,
            // ads as singleton a custom service for HTTP calls (ProtectedResourceService),
            // and registers a configuration instance with the client credentials options that will be used the ProtectedResourceService
            services.AddHttpClientService()
                    .AddSingleton<IProtectedResourceService, ProtectedResourceService>()
                    .Configure<ProtectedResourceClientCredentialsOptions>(Configuration.GetSection(nameof(ProtectedResourceClientCredentialsOptions)));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
