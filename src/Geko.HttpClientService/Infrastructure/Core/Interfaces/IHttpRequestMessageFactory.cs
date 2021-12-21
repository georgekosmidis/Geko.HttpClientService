namespace Geko.HttpClientService.Infrastructure;

/// <summary>
/// Abstraction for an <see cref="HttpRequestMessage"/> factory.
/// </summary>    
public interface IHttpRequestMessageFactory
{
    /// <summary>
    /// Creates a new instance of an <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <returns>An <see cref="HttpRequestMessage"/> to be used by an <see cref="HttpClient"/>.</returns>
    HttpRequestMessage CreateRequestMessage();
}
