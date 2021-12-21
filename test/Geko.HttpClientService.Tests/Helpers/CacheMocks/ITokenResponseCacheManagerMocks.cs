using Geko.HttpClientService.Infrastructure;
using IdentityModel.Client;
using Moq;

namespace Geko.HttpClientService.Tests.Helpers;

public static class ITokenResponseCacheManagerMocks
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
