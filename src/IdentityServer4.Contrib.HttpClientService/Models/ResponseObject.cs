using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdentityServer4.Contrib.HttpClientService.Models
{
    /// <summary>
    /// An object containing information about the response of the current request.
    /// </summary>
    /// <typeparam name="TResponseBody">Type of the body of the response.</typeparam>
    public class ResponseObject<TResponseBody>
    {
        /// <summary>
        /// The <see cref="HttpResponseHeaders"/>.
        /// </summary>
        public HttpResponseHeaders Headers { get; internal set; }

        /// <summary>
        /// The body of the response converted to <typeparamref name="TResponseBody"/>.
        /// </summary>
        public TResponseBody BodyAsType { get; internal set; }

        /// <summary>
        /// The body of the response as <see cref="String"/>.
        /// </summary>
        public string BodyAsString { get; internal set; }

        /// <summary>
        /// The body of the response as <see cref="Stream"/>.
        /// </summary>
        public Stream BodyAsStream { get; internal set; }

        /// <summary>
        /// The <see cref="HttpStatusCode"/> of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }        

        /// <summary>
        /// The entire <see cref="System.Net.Http.HttpResponseMessage"/> object.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; internal set; }

        /// <summary>
        /// The entire <see cref="HttpRequestMessage"/> object for debugging purposes.
        /// </summary>
        /// <remarks>
        /// It will be null if <see cref="HttpClientService.Dispose" /> is called for cleanup.
        /// </remarks>
        public HttpRequestMessage HttpRequestMessge { get; internal set; }

        /// <summary>
        /// A boolean indicating if there is an error in the current request.
        /// </summary>
        public bool HasError { get; internal set; } = false;

        /// <summary>
        /// A description of the error, if any.
        /// </summary>
        public string Error { get; internal set; }

    }
}