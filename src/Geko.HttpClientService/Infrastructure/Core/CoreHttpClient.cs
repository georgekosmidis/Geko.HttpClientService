namespace Geko.HttpClientService.Infrastructure;

/// <summary>
/// Implementation for a typed <see cref="HttpClient"/>.
/// </summary>
public class CoreHttpClient : ICoreHttpClient
{
    private readonly HttpClient _httpClient;
    private TimeSpan Timeout { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Constructor of the <see cref="CoreHttpClient"/>.
    /// </summary>
    /// <param name="httpClient">An <see cref="HttpClient"/> that will execute the request</param>
    public CoreHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Set the timeout for the next request.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    public void SetTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
    }

    /// <summary>
    /// Sends an <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="httpRequestMessage">The <see cref="HttpResponseMessage"/> to be sent.</param>
    /// <returns>An <see cref="HttpResponseMessage"/>.</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
    {
        var cts = new CancellationTokenSource(); ;
        if (Timeout != TimeSpan.Zero)
        {
            cts.CancelAfter(Timeout);
        }
        return await _httpClient.SendAsync(httpRequestMessage, cts.Token);
    }

}
