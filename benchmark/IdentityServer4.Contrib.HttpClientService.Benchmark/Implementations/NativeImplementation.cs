using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Benchmark.Implementations
{
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
                throw new InvalidOperationException(identityServerResponse.Error);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);
            var apiResponse = await httpClient.GetAsync("https://demo.identityserver.io/api/test");

            if (!apiResponse.IsSuccessStatusCode)
                throw new InvalidOperationException(identityServerResponse.Error);

        }
    }
}
