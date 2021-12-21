using Geko.HttpClientService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Geko.HttpClientService.Infrastructure;

/// <summary>
/// A <see cref="HttpRequestMessage"/> factory.
/// </summary>
public class HttpRequestMessageFactory : IHttpRequestMessageFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClientServiceOptions _options;

    /// <summary>
    /// Constructor of the  <see cref="HttpRequestMessageFactory"/>
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="HttpContextAccessor"/> object to access <see cref="HttpContext"/>. </param>
    /// <param name="options">The HttpClientService options to use. </param>
    public HttpRequestMessageFactory(IHttpContextAccessor httpContextAccessor, IOptions<HttpClientServiceOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options != null ? options.Value : new HttpClientServiceOptions();
    }

    /// <summary>
    /// Adds an <c>X-HttpClientService</c> header in the newly created <see cref="HttpRequestMessage"/> by copying the <c>X-HttpClientService</c> of the current request.
    /// If there is no current request or the current request does not contain a <c>X-HttpClientService</c> header, it creates a new one.
    /// </summary>
    /// <remarks>This is useful to track in the logs a series of cascading requests between services.</remarks>
    /// <returns>An <see cref="HttpRequestMessage"/> to be used by an <see cref="HttpClient"/>.</returns>
    public HttpRequestMessage CreateRequestMessage()
    {
        if (!_options.HeaderCollerationIdActive)
        {
            return new HttpRequestMessage();
        }

        var request = new HttpRequestMessage();
        var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;

        if (headers != default)
        {
            if (headers.ContainsKey(_options.HeaderCollerationName) && !string.IsNullOrWhiteSpace(headers[_options.HeaderCollerationName].ToString()))
            {
                request.Headers.Add(_options.HeaderCollerationName, headers[_options.HeaderCollerationName].ToString());
            }
            else
            {
                request.Headers.Add(_options.HeaderCollerationName, Guid.NewGuid().ToString());
            }
        }
        else
        {
            request.Headers.Add(_options.HeaderCollerationName, Guid.NewGuid().ToString());
        }

        return request;
    }

}
