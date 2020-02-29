using IdentityServer4.HttpClientService.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace IdentityServer4.HttpClientService.Extensions
{
    /// <summary>
    /// Static object for <see cref="HttpClientService" /> extensions.
    /// </summary>
    public static class HttpClientServiceHeadExtensions
    {

        /// <summary>
        /// Sends a HEAD request to the specified <paramref name="requestUri"/> and returns the response wrapped in a <see cref="ResponseObject{TResponseBody}"/> in an asynchronous operation.
        /// 
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
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended</param>
        /// <param name="requestUri">A string representing the resource to be called</param>
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
        public static async Task<ResponseObject<TResponseBody>> HeadAsync<TResponseBody>(this HttpClientService httpClientService, string requestUri)
        {
            return await httpClientService.SendAsync<object, TResponseBody>( new Uri(requestUri), HttpMethod.Head, null);
        }


        /// <summary>
        /// Sends a HEAD request to the specified <paramref name="requestUri"/> and returns the response wrapped in a <see cref="ResponseObject{TResponseBody}"/> in an asynchronous operation.
        /// </summary>
        /// <param name="httpClientService">The <see cref="HttpClientService"/> that gets extended</param>
        /// <param name="requestUri">A string representing the resource to be called</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response 
        /// as <c>String</c> in the <see cref="ResponseObject{TBody}.BodyAsString"/> property.
        /// </returns>
        public static async Task<ResponseObject<string>> HeadAsync(this HttpClientService httpClientService, string requestUri)
        {
            return await httpClientService.SendAsync<object, string>(new Uri(requestUri), HttpMethod.Head, null);
        }

    }
}