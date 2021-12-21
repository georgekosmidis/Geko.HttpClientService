using System.Net;
using System.Net.Http.Headers;

namespace Geko.HttpClientService.Models;

/// <summary>
/// An object containing information about the response of the current request.
/// </summary>
/// <typeparam name="TResponseBody">Type of the body of the response.</typeparam>
public class ResponseObject<TResponseBody>
{
    /// <summary>
    /// The <see cref="HttpResponseHeaders"/>.
    /// </summary>
    public HttpResponseHeaders? Headers { get; set; }

    /// <summary>
    /// The body of the response converted to <typeparamref name="TResponseBody"/>.
    /// </summary>
    public TResponseBody? BodyAsType { get; set; }

    /// <summary>
    /// The body of the response as <see cref="string"/>.
    /// </summary>
    public string? BodyAsString { get; set; }

    /// <summary>
    /// The body of the response as <see cref="Stream"/>.
    /// </summary>
    public Stream? BodyAsStream { get; set; }

    /// <summary>
    /// The <see cref="HttpStatusCode"/> of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// The entire <see cref="System.Net.Http.HttpResponseMessage"/> object.
    /// </summary>
    public HttpResponseMessage? HttpResponseMessage { get; set; }

    /// <summary>
    /// The entire <see cref="HttpRequestMessage"/> object for debugging purposes.
    /// </summary>
    public HttpRequestMessage? HttpRequestMessge { get; set; }

    /// <summary>
    /// A boolean indicating if there is an error in the current request.
    /// </summary>
    public bool HasError { get; set; } = false;

    /// <summary>
    /// A description of the error, if any.
    /// </summary>
    public string? Error { get; set; }

}
