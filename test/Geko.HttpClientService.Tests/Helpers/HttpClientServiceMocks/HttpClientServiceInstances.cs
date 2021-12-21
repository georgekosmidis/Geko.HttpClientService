using Geko.HttpClientService.Infrastructure;
using Geko.HttpClientService.Models;
using Microsoft.Extensions.Options;

namespace Geko.HttpClientService.Tests.Helpers;

public static class HttpClientServiceInstances
{
    public static async Task<HttpClientService> GetNew(HttpStatusCode coreStatusCode, string coreContent, bool validTokenResponse)
    {
        var httpClientService = new HttpClientServiceFactory(
            new CoreHttpClient(
                IHttpClientFactoryMocks.Get(coreStatusCode, coreContent).CreateClient()
            ),
            new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(new HttpClientServiceOptions())
            ),
            new IdentityServerService(
                new IdentityServerHttpClientSelector(
                    new List<IIdentityServerHttpClient> {
                            {
                                new ClientCredentialsHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                                )
                            },
                            {
                                new PasswordHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                                )
                            }
                    }
                ),
                ITokenResponseCacheManagerMocks.Get(
                    validTokenResponse
                    ? await TokenResponseObjects.GetValidTokenResponseAsync("access_token", 5)
                    : await TokenResponseObjects.GetInvalidTokenResponseAsync("invalid_client")
                )
            )
        ).CreateHttpClientService();

        return httpClientService;
    }

    public static async Task<HttpClientService> GetNew(HttpStatusCode coreStatusCode, Stream coreContent, bool validTokenResponse)
    {
        var httpClientService = new HttpClientServiceFactory(
            new CoreHttpClient(
                IHttpClientFactoryMocks.Get(coreStatusCode, coreContent).CreateClient()
            ),
            new HttpRequestMessageFactory(
                IHttpContextAccessorMocks.Get(),
                Options.Create(new HttpClientServiceOptions())
            ),
            new IdentityServerService(
                new IdentityServerHttpClientSelector(
                    new List<IIdentityServerHttpClient> {
                            {
                                new ClientCredentialsHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                                )
                            },
                            {
                                new PasswordHttpClient(
                                    IHttpClientFactoryMocks.Get(HttpStatusCode.OK).CreateClient()
                                )
                            }
                    }
                ),
                ITokenResponseCacheManagerMocks.Get(
                    validTokenResponse
                    ? await TokenResponseObjects.GetValidTokenResponseAsync("access_token", 3600)
                    : await TokenResponseObjects.GetInvalidTokenResponseAsync("invalid_client")
                )
            )
        ).CreateHttpClientService();

        return httpClientService;

    }

}
