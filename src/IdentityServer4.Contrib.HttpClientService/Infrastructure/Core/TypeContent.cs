using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Infrastructure
{
    /// <summary>
    /// TypeContent is following the implementation of <see cref="StringContent"/>.
    /// Copied from from https://github.com/microsoft/referencesource/blob/master/System/net/System/Net/Http/StringContent.cs
    /// </summary> 
    public class TypeContent<TRequestBody> : ByteArrayContent
    {
        private const string defaultMediaType = "application/json";
        private static readonly Encoding defaultStringEncoding = Encoding.UTF8;

        /// <summary>
        /// Creates a new instance of the <see cref="TypeContent{TRequestBody}"/> class. 
        /// Default encoding is "UTF8" and media type "application/json".
        /// </summary>
        /// <param name="model">The object of type <typeparamref name="TRequestBody"/> used to initialize the <see cref="TypeContent{TRequestBody}"/>.</param>
        public TypeContent(TRequestBody model)
            : this(model, null, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TypeContent{TRequestBody}"/> class.
        /// Default media type "application/json".
        /// </summary>
        /// <param name="model">The object of type <typeparamref name="TRequestBody"/> used to initialize the <see cref="TypeContent{TRequestBody}"/>.</param>
        /// <param name="encoding">The encoding to use for the json that will be added as content.</param>
        public TypeContent(TRequestBody model, Encoding encoding)
            : this(model, encoding, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TypeContent{TRequestBody}"/> class.
        /// </summary>
        /// <param name="model">The object of type <typeparamref name="TRequestBody"/> used to initialize the <see cref="TypeContent{TRequestBody}"/>.</param>
        /// <param name="encoding">The encoding to use for the json that will be added as content.</param>
        /// <param name="mediaType">The media type to use for the json that will be added as content.</param>
        public TypeContent(TRequestBody model, Encoding encoding, string mediaType)
            : base(GetContentByteArray(model, encoding))
        {
            // Initialize the 'Content-Type' header with information provided by parameters. 
            var headerValue = new MediaTypeHeaderValue((mediaType == null) ? defaultMediaType : mediaType);
            headerValue.CharSet = (encoding == null) ? defaultStringEncoding.WebName : encoding.WebName;

            Headers.ContentType = headerValue;
        }


        private static byte[] GetContentByteArray(TRequestBody model, Encoding encoding)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (encoding == null)
            {
                encoding = defaultStringEncoding;
            }

            //todo: JsonSerializerSettings
            var content = JsonConvert.SerializeObject(model, Formatting.None);

            return encoding.GetBytes(content);
        }
    }
}
