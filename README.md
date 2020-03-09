# An HttpClient service for IdentityServer4

[![Build Status](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_apis/build/status/georgekosmidis.IdentityServer4.Contrib.HttpClientService?branchName=master)](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_build/latest?definitionId=5&branchName=master) ![Nuget](https://img.shields.io/nuget/v/IdentityServer4.Contrib.HttpClientService)

An HttpClient service that makes it easy to make authenticated HTTP requests to protected by IdentityServer4 resources. Complex types are automatically serialized for requests /  deserialized for responses, all with a fluent interface design:

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          .CreateHttpClientService()
                          //Also supports IOptions<>
                          .SetIdentityServerOptions("appsettings_section")
                          //GET and deserialize the response body to IEnumerable<Customers>
                          .GetAsync<IEnumerable<Customers>>("https://api/customers");
```

___

##### Table of Contents
1. [Getting started](#getting-started)
   1. [It’s a nuget package!](#its-a-nuget-package)
   2. [IdentityServer4 access token request options](#identityserver4-access-token-request-options)
   3. [Register the service](#register-the-service)
   4. [You are done!](#you-are-done)
2. [How to setup an Access Token Request](#how-to-setup-an-access-token-request)
   1. [.SetIdentityServerOptions(String)](#setidentityserveroptionsstring)
   2. [.SetIdentityServerOptions&lt;TOptions&gt;(TOptions)](#setidentityserveroptionstoptions)
   3. [.SetIdentityServerOptions&lt;TOptions&gt;(IOptions&lt;TOptions&gt;)](#setidentityserveroptionsioptions)
   4. [.SetIdentityServerOptions&lt;TOptions&gt;(Action&lt;TOptions&gt;)](#setidentityserveroptionsaction)
3. [More info on how to serialize request, deserialize response](#more-info-on-how-to-serialize-request-deserialize-response)
   1. [ResponseObject](#responseobject)
   2. [TypeContent(TRequestBody, Encoding, string)](#typecontenttrequestbody-encoding-string)
4. [Contributing](#contributing)

___

## Getting started

Getting started with IdentityServer4.Contrib.HttpClientService is rather easy, you only need three things:

 1. Install the nuget package [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService)
 2. Provide the options to authenticate in `appsettings.json`
 3. Register the service in `Startup.cs`

### It's a nuget package!

Install the [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService) nuget package, using your favorite way.

### IdentityServer4 access token request options

Add the IdentityServer4 client credentials options to your `appsettings.json` (you could very well skip this step if you use one of [the other ways of setting up an access token request](#how-to-setup-an-access-token-request))

```json
"SomeClientCredentialsOptions": {
    "Address": "https://demo.identityserver.io/connect/token",
    "ClientId": "m2m",
    "ClientSecret": "secret",
    "Scopes": "api"
  }
```

*The values above are part of the demo offered in <https://demo.identityserver.io/>*

### Register the service

Register the service In `StartUp.cs` in `ConfigureServices(IServiceCollection services)`:

```csharp
services.AddHttpClientService();
```

### You are done!

Inject the `IHttpClientServiceFactory` wherever you want to make the an authenticated requests:

```csharp
public class ProtectedResourceService {

  private readonly IHttpClientServiceFactory _requestServiceFactory;

  public ProtectedResourceService(IHttpClientServiceFactory requestServiceFactory)
  {
    _requestServiceFactory = requestServiceFactory;
  }

  public async Task<IEnumerable<Customer>> GetCustomers(){
    var response = await _requestServiceFactory
      .CreateHttpClientService()
      .SetIdentityServerOptions("SomeClientCredentialsOptions")
      .GetAsync<IEnumerable<Customer>>("https://protected_resource_that_returns_customers_in_json");
  }
}
```

> The `.SetIdentityServerOptions("SomeClientCredentialsOptions")` might be simplest way of setting up an [Access Token Request](#how-to-setup-an-access-token-request), the [options pattern](#setidentityserveroptionsioptions) though is the most common one.

___

## How to setup an Access Token Request

The library supports multiple ways for setting up the necessary options for retrieving an access token. Upon success of retrieving one, the result is cached until the token expires; that means that a new request to a protected resource does not necessarily means a new request for an access token.

> Currently, the library supports `ClientCredentialsTokenRequest` and `PasswordTokenRequest`.

### .SetIdentityServerOptions(String)

Setup IdentityServer options by defining the configuration section. The type of the options (`ClientCredentialsTokenRequest` or `PasswordTokenRequest`) will be determined based on the contents of this section:

```csharp
//...
.SetIdentityServerOptions("appsettings_section")
//...
```

### .SetIdentityServerOptions&lt;TOptions&gt;(TOptions)

Setup IdentityServer options by passing a `ClientCredentialsOptions` or `PasswordOptions` directly to the `SetIdentityServerOptions`:

```csharp
//...
.SetIdentityServerOptions(
  new PasswordOptions
  {
    Address = "https://demo.identityserver.io/connect/token",
    ClientId = "ClientId",
    ClientSecret = "ClientSecret",
    Scope = "Scope",
    Username = "Username",
    Password = "Password"
  }
)
//...
```

### .SetIdentityServerOptions&lt;TOptions&gt;(IOptions&lt;TOptions&gt;)

Setup IdentityServer options using the [options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options):

```csharp
//...
public class ProtectedResourceService
{
  private readonly IHttpClientServiceFactory _requestServiceFactory;
  private readonly IOptions<ProtectedResourceClientCredentialsOptions> _identityServerOptions;

  public ProtectedResourceService(IHttpClientServiceFactory requestServiceFactory, IOptions<ProtectedResourceClientCredentialsOptions> identityServerOptions)
  {
    _requestServiceFactory = requestServiceFactory;
    _identityServerOptions = identityServerOptions;
  }

  public async Task<YourObject> Get(){
    _requestServiceFactory
    .CreateHttpClientService()
    .SetIdentityServerOptions(_identityServerOptions)
    .GetAsync<YourObject>("https://url_that_returns_YourObject");
  }
)
//...
```

### .SetIdentityServerOptions&lt;TOptions&gt;(Action&lt;TOptions&gt;)
Setup IdentityServer options using a delegate:

```csharp
//...
.SetIdentityServerOptions<PasswordOptions>( x => {
    x.Address = "https://demo.identityserver.io/connect/token";
    x.ClientId = "ClientId";
    x.ClientSecret = "ClientSecret";
    x.Scope = "Scope";
    x.Username = "Username";
    x.Password = "Password";
  }
)
//...
```

___

## More info on how to serialize request, deserialize response

Responses can always be deserialized to the typed defined in, for example, `GetAsync`:

```csharp
//...
.GetAsync<ResponsePoco>("https://url_that_returns_ResponsePoco_in_json");
//...
```

Setting up complex request body for POST, PUT and PATCH requests is also very easy:

```csharp
//...
.PostAsync<RequestPoco,ResponsePoco>("https://url_that_accepts_RequestPoco_and_responds_with_ResponsePoco", responsePoco);
//...
```

### ResponseObject

The variable **[responseObject](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html)** contains multiple properties: from the entire `HttpResponseMessage` and `HttpRequestMessage`, to the `HttpStatusCode` and `HttpResponseHeaders`. The most *exciting* feature though, is the `TResponseBody BodyAsType` property which will contain deserializabled complex types from JSON responses (in the example above the `BodyAsType` will be of type `IEnumerable<Customers>`). Check the [ResponseObject&lt;TResponseBody&gt;](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html) in the docs.

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          .CreateHttpClientService()
                          //.PostAsync<TRequestBody,TResponseBody>(URL, customer of type Customer1)
                          .PostAsync<Customer1,Customer2>("https://api/customers", customer);
```
> Without using `TypeContent(...)` to explitily set media-type and encoding, the defaults will be used: 'application/json' and 'UTF-8'

### TypeContent(TRequestBody, Encoding, string)
You can also fine tune encoding and media-type by using the `TypeContent(TRequestBody model, Encoding encoding, string mediaType)` like this:

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          .CreateHttpClientService()
                          //.PostAsync<TRequestBody,TResponseBody>(URL, customer of type Customer1)
                          .PostAsync<TypeContent<Customer1>,Customer2>("https://api/customers", new TypeContent(customer, Encoding.UTF8, "application/json"));
```

HTTP verbs supported are: [GET](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html). Read the [Documentation](api/index.md) for technical details, check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).

___

## Contributing

Feedback and contibution is more than welcome, as there are many more things to do!

Just as a sample:

1. Expand the [IdentityServer4.Contrib.HttpClientService.CompleteSample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample) with more functionality.
2. Support `JsonSerializerSettings` for `JsonConvert.DeserializeObject<TResponseBody>(apiResponse.BodyAsString)` in [HttpClientService]( https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/blob/86262f016173bafd2c9ec4fbe70ac9eb1406042a/src/IdentityServer4.Contrib.HttpClientService/HttpClientService.cs#L300).
3. Support more than `ClientCredentialsOptions` and `PasswordOptions`.
4. Set options for changing the x-header name
5. Add logging.

Many more are coming soon and all of them should be [issues](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/issues), so feel free to open one and let's start discussing solutions!
