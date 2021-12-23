using Geko.HttpClientService.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Geko.HttpClientService;

/// <summary>
/// A factory that creates <see cref="HttpClientService"/> instances for a given logical name.
/// </summary>
public sealed class HttpClientServiceFactory : IHttpClientServiceFactory
{
    private readonly ICoreHttpClient _coreHttpClient;
    private readonly IIdentityServerService _tokenResponseService;
    private readonly IHttpRequestMessageFactory _requestMessageFactory;

    private static readonly Lazy<HttpClientServiceFactory> lazyInstance
        = new(() => new HttpClientServiceFactory());

    /// <summary>
    /// Lazy Singleton instantiation for use outside of a DI container.
    /// </summary>
    public static HttpClientServiceFactory Instance => lazyInstance.Value;

    private HttpClientServiceFactory()
    {
        HttpClient? identityServerHttpClient = new();

        _coreHttpClient = new CoreHttpClient(
                            new HttpClient()
                          );

        _tokenResponseService = new IdentityServerService(
                                    new IdentityServerHttpClientSelector(
                                        new List<IIdentityServerHttpClient>
                                        {
                                                { new ClientCredentialsHttpClient(identityServerHttpClient) },
                                                { new PasswordHttpClient(identityServerHttpClient) }
                                        }
                                    ),
                                    new TokenResponseCacheManager(
                                        new MemoryCache(
                                            Options.Create(
                                                new MemoryCacheOptions()
                                            )
                                        )
                                    )
                                );

        _requestMessageFactory = new HttpRequestMessageFactoryDesktop();
    }

    /// <summary>
    /// Constructor of the <see cref="HttpClientServiceFactory" />.
    /// </summary>
    /// <param name="coreHttpClient">An <see cref="ICoreHttpClient"/> implementation that will execute the HTTP requests.</param>
    /// <param name="requestMessageFactory">The <see cref="IHttpRequestMessageFactory"/> to get a new <see cref="HttpRequestMessage"/>.</param>
    /// <param name="tokenResponseService">The <see cref="IIdentityServerService"/> to retrieve a token, if required.</param>
    public HttpClientServiceFactory(ICoreHttpClient coreHttpClient, IHttpRequestMessageFactory requestMessageFactory, IIdentityServerService tokenResponseService)
    {
        _coreHttpClient = coreHttpClient ?? throw new ArgumentNullException(nameof(coreHttpClient));
        _tokenResponseService = tokenResponseService ?? throw new ArgumentNullException(nameof(tokenResponseService));
        _requestMessageFactory = requestMessageFactory ?? throw new ArgumentNullException(nameof(requestMessageFactory));
    }

    /// <summary>
    /// Creates new <see cref="HttpClientService"/> instances. 
    /// </summary>
    /// <returns>An <see cref="HttpClientService"/> instance.</returns>
    public HttpClientService CreateHttpClientService()
    {
        return new HttpClientService(_coreHttpClient, _requestMessageFactory, _tokenResponseService);
    }
}
