using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Models;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization;
using IdentityModel.Client;

namespace IdentityServer4.Contrib.HttpClientService
{
    /// <summary>
    /// The request service implemantation
    /// </summary>
    public class HttpClientService
    {
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerService _accessTokenService;
        private readonly ICoreHttpClient _coreHttpClient;
        private readonly IHttpRequestMessageFactory _requestMessageFactory;

        private IIdentityServerOptions identityServerOptions;
        private HttpClientServiceHeaders headers { get; }

        internal HttpClientService(IConfiguration configuration, ICoreHttpClient coreHttpClient, IHttpRequestMessageFactory requestMessageFactory, IIdentityServerService accessTokenService)
        {
            _configuration = configuration;
            _coreHttpClient = coreHttpClient;
            _requestMessageFactory = requestMessageFactory;
            _accessTokenService = accessTokenService;

            headers = new HttpClientServiceHeaders();
        }

        #region Headers
        /// <summary>
        /// Adds a header to the request. If a second header with the same name is added, then their values will be aggregated to one <see cref="List{String}"/>.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>

        public HttpClientService HeadersAdd(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            headers.Add(name, value);

            return this;
        }

        /// <summary>
        /// Adds a header to the request. If a second header with the same name is added, then their values will be aggregated to one <see cref="List{TString}"/>.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The list of values of the header.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService HeadersAdd(string name, List<string> value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            headers.Add(name, value);

            return this;
        }

        /// <summary>
        /// Clears all current headers, and sets the <paramref name="headers"/> as the headers for the request.
        /// </summary>
        /// <param name="headers">A <see cref="Dictionary{TKey, TValue}"/> with the key representing the name of the header, and the value representing the value of the header.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>

        public HttpClientService HeadersSet(Dictionary<string, List<string>> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            HeadersClear();

            foreach (var kv in headers)
            {
                this.headers.Add(kv.Key, kv.Value);
            }

            return this;
        }

        /// <summary>
        /// Clears all current headers, and sets the <paramref name="headers"/> as the headers for the request.
        /// </summary>
        /// <param name="headers">A <see cref="Dictionary{TKey, TValue}"/> where value is a <see cref="List{TString}"/> with the key representing the name of the header, and the values representing the values of the header.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService HeadersSet(Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            HeadersClear();

            foreach (var kv in headers)
            {
                this.headers.Add(kv.Key, kv.Value);
            }

            return this;
        }

        /// <summary>
        /// Clears all headers from the <see cref="HttpHeaders"/> collection.
        /// </summary>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService HeadersClear()
        {
            headers.Clear();
            return this;
        }

        /// <summary>
        /// Removes the <paramref name="name"/> header from the <see cref="HttpHeaders"/> collection.
        /// </summary>
        /// <param name="name">The header to be removed</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService HeadersRemove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            headers.Remove(name);
            return this;
        }

        #endregion

        #region IdentityServer
        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token s by passing the appsettings configuration section 
        /// that contain the necessary configuration keys. The section name should be or end with 
        /// either <see cref="ClientCredentialsOptions"/> or <see cref="PasswordOptions"/>.
        /// </summary>
        /// <param name="configurationSection">
        /// The name of the configuration section that contains the information for requesting an access token. 
        /// It should be or end with either <see cref="ClientCredentialsOptions"/> or <see cref="PasswordOptions"/>.
        /// </param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService SetIdentityServerOptions(string configurationSection)
        {
            if (_configuration == null)
                throw new InvalidOperationException("The string configuraton cannot be used with the the lazy singleton instance of " + nameof(HttpClientService) + " (" + nameof(HttpClientServiceFactory) + "." + nameof(HttpClientServiceFactory.Instance) + ")");

            var sectionExists = _configuration.GetChildren().Any(item => item.Key == configurationSection);
            if (!sectionExists)
                throw new ArgumentException("The configuration section '" + configurationSection + "' cannot be found!", nameof(configurationSection));

            //todo: find better way
            if (configurationSection.ToLower().EndsWith(nameof(ClientCredentialsOptions).ToLower()))
            {
                identityServerOptions = _configuration.GetSection(configurationSection).Get<ClientCredentialsOptions>();
            }
            else if (configurationSection.ToLower().EndsWith(nameof(PasswordOptions).ToLower()))
            {
                identityServerOptions = _configuration.GetSection(configurationSection).Get<PasswordOptions>();
            }
            else
            {
                throw new InvalidOperationException("The name or the suffix of the cofiguration section must be either '" + nameof(ClientCredentialsOptions) + "' or '" + nameof(PasswordOptions) + "'.");
            }

            return this;
        }

        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token using client credentials.
        /// </summary>
        /// <typeparam name="TOptions">The type of the <paramref name="options"/>.</typeparam>
        /// <param name="options">The token service options.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService SetIdentityServerOptions<TOptions>(IOptions<TOptions> options)
            where TOptions : class, IIdentityServerOptions, new()
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            identityServerOptions = options.Value;
            return this;
        }

        /// <summary>
        /// Sets the IdentityServer4 options for retrieving an access token using client credentials by using a delegate.
        /// </summary>
        /// <param name="optionsDelegate">The <see cref="Action{T}"/> delegate.</param>
        /// <returns>Returns the current instance of <see cref="HttpClientService"/> for method chaining.</returns>
        public HttpClientService SetIdentityServerOptions<TOptions>(Action<TOptions> optionsDelegate)
            where TOptions : class, IIdentityServerOptions
        {
            identityServerOptions = Activator.CreateInstance(typeof(TOptions)) as TOptions;
            optionsDelegate(identityServerOptions as TOptions);
            return this;
        }

        /// <summary>
        /// Sets the IdentityServer4 options by passing an object that inherits from <see cref="ClientCredentialsOptions"/>
        /// </summary>
        /// <param name="options">The <see cref="IIdentityServerOptions"/> that contains the options.</param>
        public HttpClientService SetIdentityServerOptions<TOptions>(TOptions options)
            where TOptions : class, IIdentityServerOptions
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            identityServerOptions = options;
            return this;
        }

        /// <summary>
        /// Retrieves the TokenResponse from the IdentityServer
        /// </summary>
        /// <returns>A <see cref="TokenResponse"/></returns>
        public async Task<TokenResponse> GetTokenResponse()
        {
            if (identityServerOptions == null)
            {
                throw new InvalidOperationException("IdentityServerOptions have not been set. Please set the options with one of the SetIdentityServerOptions overloads first.");
            }

            return await _accessTokenService.GetTokenResponseAsync(identityServerOptions);
        }
        #endregion

        /// <summary>
        /// Creates and sends a request to the <paramref name="requestUri"/> using the HTTP verb 
        ///  from <paramref name="httpMethod"/> and the request body from <paramref name="requestBody"/>. 
        /// If IdentityServerOptions are set (using the <see cref="SetIdentityServerOptions{TOptions}(TOptions)"/> or one of the overloads), 
        /// then a valid access token will be fetched by the <see cref="IIdentityServerService"/> and attached to this request. 
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
        /// <param name="requestUri">The <see cref="Uri"/> of the request.</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> of the request.</param>
        /// <param name="requestBody">The body of the request (available only in POST, PUT and PATCH).</param>
        /// <returns>
        /// A <see cref="ResponseObject{TResponseBody}"/> containing the body of the response 
        /// as <c>String</c> in the <see cref="ResponseObject{TResponseBody}.BodyAsString"/> property,
        /// as <typeparamref name="TResponseBody"/> in the <see cref="ResponseObject{TBody}.BodyAsType"/> and,
        /// as <c>Stream</c> in the <see cref="ResponseObject{TResponseBody}.BodyAsStream"/> property.
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
        public async Task<ResponseObject<TResponseBody>> SendAsync<TRequestBody, TResponseBody>(Uri requestUri, HttpMethod httpMethod, TRequestBody requestBody)
        {
            var httpRequestMessage = _requestMessageFactory.CreateRequestMessage();

            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = requestUri;

            //todo: unit test that
            if ((httpMethod == HttpMethod.Get || httpMethod == HttpMethod.Head || httpMethod == HttpMethod.Delete)
                && requestBody != null)
                throw new ArgumentException(nameof(requestBody), "HTTP method " + httpMethod.Method + " does not support a request body.");

            //headers
            foreach (var kv in headers)
            {
                httpRequestMessage.Headers.Add(kv.Key, kv.Value);
            }

            //handle request body
            if (requestBody != null)
            {
                //HttpContent types
                if (requestBody as HttpContent != null)
                {
                    httpRequestMessage.Content = requestBody as HttpContent;
                }
                //no HttpContent types, wrap in an HttpContent type with default encoding and content-type
                else
                {
                    //utf-8, text/plain
                    if (IsSimpleType(typeof(TRequestBody)))
                    {
                        httpRequestMessage.Content = new StringContent(requestBody.ToString());
                    }
                    //utf-8, application/json
                    else
                    {
                        httpRequestMessage.Content = new TypeContent<TRequestBody>(requestBody);
                    }
                }
            }

            //handle authentication
            if (identityServerOptions != null)
            {
                var tokenResponse = await _accessTokenService.GetTokenResponseAsync(identityServerOptions);
                if (tokenResponse.IsError)
                {
                    return new ResponseObject<TResponseBody>
                    {
                        StatusCode = tokenResponse.HttpStatusCode,
                        HttpResponseMessage = tokenResponse.HttpResponse,
                        HttpRequestMessge = null,
                        HasError = true,
                        Error = tokenResponse.Error + Environment.NewLine + tokenResponse.ErrorDescription,
                        BodyAsString = await tokenResponse.HttpResponse.Content.ReadAsStringAsync()
                    };
                }

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
            }

            //make the call            
            var response = await _coreHttpClient.SendAsync(httpRequestMessage);
            var apiResponse = new ResponseObject<TResponseBody>
            {
                Headers = response.Headers,
                StatusCode = response.StatusCode,
                HttpResponseMessage = response,
                HttpRequestMessge = httpRequestMessage
            };

            //handle response body
            if (response.IsSuccessStatusCode)
            {
                if (!typeof(TResponseBody).IsAssignableFrom(typeof(Stream)))
                {
                    apiResponse.BodyAsString = await response.Content.ReadAsStringAsync();
                    //try to convert to the requested type
                    if (IsSimpleType(typeof(TResponseBody)))
                    {
                        apiResponse.BodyAsType = (TResponseBody)Convert.ChangeType(apiResponse.BodyAsString, typeof(TResponseBody));
                    }
                    else
                    {
                        apiResponse.BodyAsType = JsonConvert.DeserializeObject<TResponseBody>(apiResponse.BodyAsString);
                    }
                }
                else
                {
                    apiResponse.BodyAsStream = await response.Content.ReadAsStreamAsync();
                }
            }
            else
            {
                apiResponse.HasError = true;
                apiResponse.Error = response.ReasonPhrase;
                apiResponse.BodyAsString = await response.Content.ReadAsStringAsync();
            }

            return apiResponse;
        }


        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(Enum),
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }
    }
}