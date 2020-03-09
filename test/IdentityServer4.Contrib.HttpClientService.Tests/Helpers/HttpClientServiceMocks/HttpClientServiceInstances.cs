using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public static class HttpClientServiceInstances
    {
        public static async Task<HttpClientService> GetNew(HttpStatusCode coreStatusCode, string coreContent, bool validTokenResponse)
        {
            var httpClientService = new HttpClientServiceFactory(
                GetConfigurationMock("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(coreStatusCode, coreContent).CreateClient()
                ),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
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
        public static async Task<HttpClientService> GetNew(HttpStatusCode coreStatusCode, Stream coreContent, bool validTokenResponse)
        {
            var httpClientService = new HttpClientServiceFactory(
                GetConfigurationMock("key", "section_data"),
                new CoreHttpClient(
                    IHttpClientFactoryMocks.Get(coreStatusCode, coreContent).CreateClient()
                ),
                new HttpRequestMessageFactory(
                    IHttpContextAccessorMocks.Get()
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

        private static IConfiguration GetConfigurationMock(string keyName, string sectionData)
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.IsAny<string>()]).Returns(sectionData);
            mockConfSection.SetupGet(m => m.Key).Returns(keyName);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(mockConfSection.Object);
            //mockConfiguration.Setup(a => a.Get(It.IsAny<Type>())).Returns(new List<IConfigurationSection> { mockConfSection.Object });

            return mockConfiguration.Object;
        }
    }
}
