using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;

namespace Geko.HttpClientService.Infrastructure;

/// <summary>
/// Cache manager for the IdentityServer4 <see cref="TokenResponse"/>. 
/// </summary>
/// <remarks>
/// It caches a successful response for the 75% of the expiration time define in the <see cref="TokenResponse"/>. 
/// </remarks>
public class TokenResponseCacheManager : ITokenResponseCacheManager
{
    private readonly IMemoryCache _cache;
    private static readonly SemaphoreSlim semaphoreSlim = new(1);

    /// <summary>
    /// Constructor of the <see cref="TokenResponseCacheManager"/>.
    /// </summary>
    /// <param name="memoryCache">The in-memory cache.</param>
    public TokenResponseCacheManager(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    /// <summary>
    /// Adds a new <see cref="TokenResponse"/> in the cache by executing the <paramref name="call"/> parameter, or returns an existing cached response.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="call">The delegate the returns a <see cref="TokenResponse"/>.</param>
    /// <returns>A <see cref="TokenResponse"/>, either just aqcuired or from the cache</returns>
    public async Task<TokenResponse> AddOrGetExistingAsync(string key, Func<Task<TokenResponse>> call)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Cache keys cannot be empty or whitespace", nameof(key));
        }

        if (_cache.TryGetValue(key, out TokenResponse tokenResponse))
        {
            return tokenResponse;
        }

        await semaphoreSlim.WaitAsync();

        try
        {
            if (_cache.TryGetValue(key, out tokenResponse))
            {
                return tokenResponse;
            }

            tokenResponse = await call();

            var cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn * .75),
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(key, tokenResponse, cacheExpirationOptions);

            return tokenResponse;
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// Removes an entry from the cache
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    public void Remove(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Cache keys cannot be empty or whitespace", nameof(key));
        }

        _cache.Remove(key);
    }
}
