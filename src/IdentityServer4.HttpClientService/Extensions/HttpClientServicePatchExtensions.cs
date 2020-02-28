using IdentityServer4.HttpClientService.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace IdentityServer4.HttpClientService.Extensions
{
    /// <summary>
    /// Static object for <see cref="HttpClientService" /> extensions.
    /// </summary>
    public static class HttpClientServicePatchExtensions
    {

        /// <summary>
        /// Sends a PATCH request to the specified <paramref name="requestUri"/> using <paramref name="requestBody"/> as the body of the request
        /// with <typeparamref name="TRequestBody"/> as the type of the <paramref name="requestBody"/>.
        /// Returns the response wrapped in a <see cref="ResponseObject{TResponseBody}"/>.
        /// <list type="table">
        ///  <listheader>The <see cref="ResponseObject{TResponseBody}"/> contains the body of the response as:</listheader>
        ///     <item><term><c>String</c></term><description> in the <see cref="ResponseObject{TResponseBody}.BodyAsString"/> property</description></item>
        ///     <item><term><typeparamref name="TResponseBody"/></term><description> in the <see cref="ResponseObject{TBody}.BodyAsType"/> property</description></item>
        ///     <item><term><c>Stream</c></term><description> in the <see cref="ResponseObject{TResponseBody}.BodyAsStream"/> property</description></item>
        /// </list>
        /// </summary>
        /// <typeparam name="TResponseBody">
        ///     The type of the property <see cref="ResponseObject{TResponseBody}.BodyAsType"/> of the <see cref="ResponseObject{TResponseBody}"/> object,
        ///     that will contain the body of the response deserialized or casted to type <typeparamref name="TResponseBody"/>. 
        ///     The type used can be one of the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <term><see cref="StringContent"/></term>
        ///             <description>Use <see cref="StringContent"/> to define Encoding and/or ContentType for an HTTP content based on string.</description>
        ///         </item>
        ///         <item>
        ///             <term><see cref="StreamContent"/></term>
        ///             <description>Use <see cref="StreamContent"/> to provide HTTP content based on a stream.</description>
        ///         </item>
        ///         <item>
        ///             <term>A serializable complex type</term>
        ///             <description>Any serializable object to attempt to deserialize the body of the response to it.</description>
        ///         </item>
        ///         <item>
        ///             <term>A simple type</term>
        ///             <description>Any other simple type to try convert the body of the response to it.</description>
        ///         </item>
        ///     </list>
        /// </typeparam>
        /// <typeparam name="TRequestBody">
        ///     The type of the request body. The type used can be one of the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <term><see cref="StringContent"/></term>
        ///             <description>Use <see cref="StringContent"/> to define Encoding and/or ContentType for an HTTP content based on string.</description>
        ///         </item>
        ///         <item>
        ///             <term><see cref="StreamContent"/></term>
        ///             <description>Use <see cref="StreamContent"/> to provide HTTP content based on a stream.</description>
        ///         </item>
        ///         <item>
        ///             <term>A serializable complex type</term>
        ///             <description>Any serializable object that will be serialized and sent in the body of the request.</description>
        ///         </item>
        ///         <item>
        ///             <term>A simple type</term>
        ///             <description>Any other simple type that will be sent in the body of the request.</description>
        ///         </item>
        ///     </list>
        /// </typeparam>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended.</param>
        /// <param name="requestUri">A string representing the resource to be called.</param>
        /// <param name="requestBody">The body of the request.</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response 
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property,
        /// as <typeparamref name="TResponseBody"/> in the <see cref="ResponseObject{TBody}.BodyAsType"/> and,
        /// as <c>Stream</c> in the <see cref="ResponseObject{TBody}.BodyAsStream"/> property.
        /// The <typeparamref name="TResponseBody"/> can be of the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <term><see cref="StringContent"/></term>
        ///             <description>Use <see cref="StringContent"/> to define Encoding and/or ContentType for an HTTP content based on string.</description>
        ///         </item>
        ///         <item>
        ///             <term><see cref="StreamContent"/></term>
        ///             <description>Use <see cref="StreamContent"/> to provide HTTP content based on a stream.</description>
        ///         </item>
        ///         <item>
        ///             <term>A serializable complex type</term>
        ///             <description>Any serializable object to attempt to deserialize the body of the response to it.</description>
        ///         </item>
        ///         <item>
        ///             <term>A simple type</term>
        ///             <description>Any other simple type to try convert the body of the response to it.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        public static async Task<ResponseObject<TResponseBody>> PatchAsync<TRequestBody, TResponseBody>(this HttpClientService httpClientService, string requestUri, TRequestBody requestBody)
        {
            return await httpClientService.SendAsync<TResponseBody, TRequestBody>( new Uri(requestUri), HttpMethod.Patch, requestBody);
        }

        /// <summary>
        /// Sends a PATCH request to the specified <paramref name="requestUri"/> using <paramref name="requestBody"/> as the body of the request
        /// with <typeparamref name="TRequestBody"/> as the type of the <paramref name="requestBody"/>.
        /// Returns the response in the <see cref="ResponseObject{TResponseBody}.BodyAsString"/> property.
        /// </summary>
        /// <typeparam name="TRequestBody">
        ///     The type of the request body. The type used can be one of the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <term><see cref="StringContent"/></term>
        ///             <description>Use <see cref="StringContent"/> to define Encoding and/or ContentType for an HTTP content based on string.</description>
        ///         </item>
        ///         <item>
        ///             <term><see cref="StreamContent"/></term>
        ///             <description>Use <see cref="StreamContent"/> to provide HTTP content based on a stream.</description>
        ///         </item>
        ///         <item>
        ///             <term>A serializable complex type</term>
        ///             <description>Any serializable object that will be serialized and sent in the body of the request.</description>
        ///         </item>
        ///         <item>
        ///             <term>A simple type</term>
        ///             <description>Any other simple type that will be sent in the body of the request.</description>
        ///         </item>
        ///     </list>
        /// </typeparam>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended.</param>
        /// <param name="requestUri">A string representing the resource to be called.</param>
        /// <param name="requestBody">The body of the request.</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property.
        /// </returns>
        public static async Task<ResponseObject<string>> PatchAsync<TRequestBody>(this HttpClientService httpClientService, string requestUri, TRequestBody requestBody)
        {
            return await httpClientService.SendAsync<string, TRequestBody>(new Uri(requestUri), HttpMethod.Patch, requestBody);
        }

        /// <summary>
        /// Sends a PATCH request to the specified <paramref name="requestUri"/> using <paramref name="requestBody"/> as the body of the request.
        /// Returns the response in the <see cref="ResponseObject{TResponseBody}.BodyAsString"/> property.
        /// </summary>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended.</param>
        /// <param name="requestUri">A string representing the resource to be called.</param>
        /// <param name="requestBody">The body of the request.</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property.
        /// </returns>
        public static async Task<ResponseObject<string>> PatchAsync(this HttpClientService httpClientService, string requestUri, string requestBody)
        {
            return await httpClientService.SendAsync<string, string>(new Uri(requestUri), HttpMethod.Patch, requestBody);
        }

        /// <summary>
        /// Sends a PATCH request to the specified <paramref name="requestUri"/> using <paramref name="requestBody"/> as the body of the request.
        /// Returns the response in the <see cref="ResponseObject{TResponseBody}.BodyAsString"/> property.
        /// </summary>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended.</param>
        /// <param name="requestUri">A string representing the resource to be called.</param>
        /// <param name="requestBody">The body of the request.</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property.
        /// </returns>
        public static async Task<ResponseObject<string>> PatchAsync(this HttpClientService httpClientService, string requestUri, StringContent requestBody)
        {
            return await httpClientService.SendAsync<string, StringContent>(new Uri(requestUri), HttpMethod.Patch, requestBody);
        }

        /// <summary>
        /// Sends a PATCH request to the specified <paramref name="requestUri"/> using <paramref name="requestBody"/> as the body of the request.
        /// Returns the response in the <see cref="ResponseObject{TResponseBody}.BodyAsType"/> property.
        /// </summary>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended.</param>
        /// <param name="requestUri">A string representing the resource to be called.</param>
        /// <param name="requestBody">The body of the request.</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property.
        /// </returns>
        public static async Task<ResponseObject<string>> PatchAsync(this HttpClientService httpClientService, string requestUri, Stream requestBody)
        {
            return await httpClientService.SendAsync<string, Stream>(new Uri(requestUri), HttpMethod.Patch, requestBody);
        }        
    }
}