# Documentation common methods/properties

### Class HttpClientService
https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.HttpClientService.html#IdentityServer4_Contrib_HttpClientService_HttpClientService_SetIdentityServerOptions__1_Microsoft_Extensions_Options_IOptions___0__
- [SetIdentityServerOptions(String)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.HttpClientService.html#IdentityServer4_Contrib_HttpClientService_HttpClientService_SetIdentityServerOptions_System_String_)
Sets the IdentityServer4 options for retrieving an access token using client credentials by passing the appsettings configuration section that contain the necessary configuration keys.
- [SetIdentityServerOptions<TTokenServiceOptions>(IOptions<TTokenServiceOptions>)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.HttpClientService.html#IdentityServer4_Contrib_HttpClientService_HttpClientService_SetIdentityServerOptions__1_Microsoft_Extensions_Options_IOptions___0__)
Sets the IdentityServer4 options for retrieving an access token using client credentials.

- [AddHeader(String, String)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.HttpClientService.html#IdentityServer4_Contrib_HttpClientService_HttpClientService_AddHeader_System_String_System_String_)
Adds a header to the request.

### Class HttpClientServiceFactory
- [CreateHttpClientService(String)]
Creates or returns HttpClientService instances for the given logical name.

### Class ResponseObject<TResponseBody>
- [BodyAsStream](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_BodyAsStream)
The body of the response as `System.IO.Stream`.
- [BodyAsString](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_BodyAsString)
The body of the response as `System.String`.
- [BodyAsType](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_BodyAsType)
The body of the response converted to `TResponseBody`.
- [Error](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_Error)
A description of the error, if any.
- [HasError](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_HasError)
A boolean indicating if there is an error in the current request.
- [Headers](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_Headers)
The `System.Net.Http.Headers.HttpResponseHeaders`.
- [HttpRequestMessge](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_HttpRequestMessge)
The entire `System.Net.Http.HttpRequestMessage` object for debugging purposes.
- [HttpResponseMessge](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_HttpResponseMessage)
The entire `System.Net.Http.HttpResponseMessage` object.
- [StatusCode](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html#IdentityServer4_Contrib_HttpClientService_Models_ResponseObject_1_StatusCode)
The `System.Net.HttpStatusCode` of the response.

### Extension methods for GET, POST, PUT and DELETE
- [GetAsync<TResponseBody>(HttpClientService, String)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html#IdentityServer4_Contrib_HttpClientService_Extensions_HttpClientServiceGetExtensions_GetAsync__1_IdentityServer4_Contrib_HttpClientService_HttpClientService_System_String_)
Sends a GET request to the specified requestUri and returns the response wrapped in a `ResponseObject<TResponseBody>` in an asynchronous operation.
- [PostAsync<TRequestBody, TResponseBody>(HttpClientService, String, TRequestBody)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html#IdentityServer4_Contrib_HttpClientService_Extensions_HttpClientServicePostExtensions_PostAsync__2_IdentityServer4_Contrib_HttpClientService_HttpClientService_System_String___0_)
Sends a POST request to the specified requestUri using requestBody as the body of the request with `TRequestBody` as the type of the requestBody. Returns the response wrapped in a `ResponseObject<TResponseBody>`.
- [PutAsync<TRequestBody, TResponseBody>(HttpClientService, String, TRequestBody](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html#IdentityServer4_Contrib_HttpClientService_Extensions_HttpClientServicePutExtensions_PutAsync__2_IdentityServer4_Contrib_HttpClientService_HttpClientService_System_String___0_)
Sends a PUT request to the specified requestUri using requestBody as the body of the request with TRequestBody as the type of the requestBody. Returns the response wrapped in a `ResponseObject<TResponseBody>`.
- [DeleteAsync<TResponseBody>(HttpClientService, String)](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html#IdentityServer4_Contrib_HttpClientService_Extensions_HttpClientServiceDeleteExtensions_DeleteAsync__1_IdentityServer4_Contrib_HttpClientService_HttpClientService_System_String_)
Sends a DELETE request to the specified requestUri and returns the response wrapped in a `ResponseObject<TResponseBody>` in an asynchronous operation.
