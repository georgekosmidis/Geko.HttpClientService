using IdentityModel.Client;
using IdentityServer4.HttpClientService.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.HttpClientService.Tests.Helpers
{
    public static class IAccessTokenCacheManagerMocks
    {
        public static ITokenResponseCacheManager Get(TokenResponse expectedValue)
        {
            var mockAccessTokenCacheManager = new Mock<ITokenResponseCacheManager>();
            mockAccessTokenCacheManager
                .Setup(x => x.AddOrGetExistingAsync(It.IsAny<string>(), It.IsAny<Func<Task<TokenResponse>>>()))
                .Returns(Task.FromResult(expectedValue));
           
            return mockAccessTokenCacheManager.Object;
        }        
    }
}
