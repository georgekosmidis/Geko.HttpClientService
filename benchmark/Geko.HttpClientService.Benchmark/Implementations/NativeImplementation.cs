using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Geko.HttpClientService.Benchmark.Implementations;

public static class NativeImplementation
{
    public static async Task Sample(HttpClient httpClient)
    {

        var identityServerResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = "https://demo.identityserver.io/connect/token",
            ClientId = "m2m",
            ClientSecret = "secret",
            Scope = "api",
        });

        if (identityServerResponse.IsError)
        {
            throw new Exception(identityServerResponse.Error);
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);
        var apiResponse = await httpClient.GetAsync("https://demo.identityserver.io/api/test?" + Guid.NewGuid().ToString());

        if (!apiResponse.IsSuccessStatusCode)
        {
            throw new Exception(identityServerResponse.Error);
        }

    }
}
