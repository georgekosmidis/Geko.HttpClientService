using System;
using System.Threading.Tasks;
using Geko.HttpClientService.Extensions;
using Geko.HttpClientService.Models;

namespace Geko.HttpClientService.Benchmark.Implementations;

public static class HttpClientServiceImplementation
{
    private static HttpClientService _httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();

    public static async Task Sample()
    {
        var responseObject = await _httpClientService
            .SetIdentityServerOptions<ClientCredentialsOptions>(x =>
            {
                x.Address = "https://demo.identityserver.io/connect/token";
                x.ClientId = "m2m";
                x.ClientSecret = "secret";
                x.Scope = "api";
            })
            .GetAsync("https://demo.identityserver.io/api/test?" + Guid.NewGuid().ToString());

        //all good?
        if (responseObject.HasError)
        {
            throw new Exception(responseObject.Error);
        }
    }
}
