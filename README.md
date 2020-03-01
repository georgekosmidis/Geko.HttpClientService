# An HttpClient service for IdentityServer4

An HttpClient service that makes it easy to make authenticated HTTP requests to protected by IdentityServer4 resources. Complex types are automatically serialized for requests /  deserialized form responses, all with a fluent interface design:

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          // could also be .CreateHttpClientService(), but before you ommit the name
                          // read more about HttpClient and socket exhaustion
                          .CreateHttpClientService("name_of_service")
                          //Also supports IOptions<>
                          .SetIdentityServerOptions("appsettings_section")     
                          //GET and deserialize the response body to IEnumerable<Customers>
                          .GetAsync<IEnumerable<Customers>>("https://api/customers");
```					
> Equaly simple for all HTTP verbs, check the docs for [GET](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html) or the [samples](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples)

## How to 
tbd
