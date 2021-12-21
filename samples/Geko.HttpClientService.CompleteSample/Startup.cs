using Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices;
using Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Services;
using Geko.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices;
using Geko.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices.Services;
using Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices;
using Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services;
using Geko.HttpClientService.Extensions;
using Geko.HttpClientService.Models;

namespace Geko.HttpClientService.CompleteSample;

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
        //Add the HTTP client service factory to the service collection,
        services.AddHttpClientService()
                //Configure it
                .Configure<HttpClientServiceOptions>(Configuration.GetSection(nameof(HttpClientServiceOptions)))
                //Add as singleton a custom service and configure it,
                .AddSingleton<IClientCredentialsProtectedResourceService, ClientCredentialsProtectedResourceService>()
                .Configure<OtherClientCredentialsOptions>(Configuration.GetSection(nameof(OtherClientCredentialsOptions)))
                //Add as singleton a second custom service and configure it,
                .AddSingleton<IClientCredentials2ProtectedResourceService, ClientCredentials2ProtectedResourceService>()
                .Configure<SomeClientCredentialsOptions>(Configuration.GetSection(nameof(SomeClientCredentialsOptions)));

        services.AddSingleton<IPasswordProtectedResourceService, PasswordProtectedResourceService>()
                .Configure<SomePasswordOptions>(Configuration.GetSection(nameof(SomePasswordOptions)));
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
