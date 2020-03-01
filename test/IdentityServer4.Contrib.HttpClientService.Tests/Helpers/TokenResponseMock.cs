using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public static class TokenResponseMock
    {
        public static async Task<TokenResponse> GetValidResponseAsync(string accessToken, int expiresIn)
        {
            var httpClient = IHttpClientFactoryMocks.Get(HttpStatusCode.OK,
                @"{
                    ""access_token"": """ + accessToken + @""",
                    ""expires_in"": " + expiresIn + @",
                    ""token_type"": ""Bearer""
                }").CreateClient("TokenResponseMock");

            return await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "http://localhost/"
            });
        }

        public static async Task<TokenResponse> GetErrorResponseAsync(string error)
        {
            var httpClient = IHttpClientFactoryMocks.Get(HttpStatusCode.OK,
                "{\"error\":\""+ error + "\"}").CreateClient("TokenResponseMock");

            return await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "http://localhost/"
            });
        }
    }
}
