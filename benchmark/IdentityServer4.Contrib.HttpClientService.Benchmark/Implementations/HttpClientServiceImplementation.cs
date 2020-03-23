using IdentityServer4.Contrib.HttpClientService.Extensions;
using IdentityServer4.Contrib.HttpClientService.Models;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Benchmark.Implementations
{
    public static class HttpClientServiceImplementation
    {
        public static async Task Sample()
        {
            var responseObject = await HttpClientServiceFactory
                .Instance
                .CreateHttpClientService()
                .SetIdentityServerOptions<ClientCredentialsOptions>(x =>
                {
                    x.Address = "https://demo.identityserver.io/connect/token";
                    x.ClientId = "m2m";
                    x.ClientSecret = "secret";
                    x.Scope = "api";
                })
                .GetAsync("https://demo.identityserver.io/api/test");

            //all good?
            if (responseObject.HasError)
                throw new InvalidOperationException(responseObject.Error);

        }
    }
}
