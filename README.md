# An HttpClient service for IdentityServer4

An HttpClient service that makes it easy to make authenticated HTTP requests to protected by IdentityServer4 resources. Complex types are automatically serialized for requests /  deserialized form responses, all with a fluent interface design.

## A simple approach
```csharp
var response = await _requestServiceFactory
                    .CreateHttpClientService("name_of_service")                 //Can also be .CreateHttpClientService(), but read more about HttpClient and socket exhaustion issues
                    .SetIdentityServerOptions("appsettings_section")            //Also supports IOptions
                    .GetAsync<IEnumerable<Customers>>("https://api/customers"); //GET and return as IEnumerable<Customers>
```					

## How to install
tbd
