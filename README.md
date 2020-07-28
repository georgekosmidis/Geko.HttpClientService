# An HttpClient service for IdentityServer4

[![Build Status](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_apis/build/status/georgekosmidis.IdentityServer4.Contrib.HttpClientService?branchName=master)](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_build/latest?definitionId=5&branchName=master) ![Nuget](https://img.shields.io/nuget/v/IdentityServer4.Contrib.HttpClientService)

An [2x times faster](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/benchmark) fluent HttpClient service that makes it easy to make authenticated HTTP requests to protected by IdentityServer4 resources. Complex types are automatically serialized for requests /  deserialized for responses, all with a fluent interface design:

```csharp
var responseObject = await _requestServiceFactory
	//Create a instance of the service
	.CreateHttpClientService()
	//Supports many ways of setting IdentityServer options
	.SetIdentityServerOptions("ClientCredentialsOptions")
	//GET and deserialize the response body to IEnumerable<Customers>
	.GetAsync<IEnumerable<Customers>>("https://api/customers");
```

**Check the [Getting started](#getting-started) guide for more details!**
___

##### Table of Contents
1. [Getting started](#getting-started)
   1. [It’s a nuget package!](#its-a-nuget-package)
   2. [IdentityServer4 Access Token Request Options](#identityserver4-access-token-request-options)
   3. [Register the service](#register-the-service)
   4. [You are done!](#you-are-done)
2. [How to setup an Access Token Request](#how-to-setup-an-access-token-request)
   1. [.SetIdentityServerOptions(String)](#setidentityserveroptionsstring)
   2. [.SetIdentityServerOptions&lt;TOptions&gt;(TOptions)](#setidentityserveroptionstoptionstoptions)
   3. [.SetIdentityServerOptions&lt;TOptions&gt;(IOptions&lt;TOptions&gt;)](#setidentityserveroptionstoptionsioptionstoptions)
   4. [.SetIdentityServerOptions&lt;TOptions&gt;(Action&lt;TOptions&gt;)](#setidentityserveroptionstoptionsactiontoptions)
3. [More info on how to serialize request, deserialize response](#more-info-on-how-to-serialize-request-deserialize-response)
   1. [ResponseObject](#responseobject)
   2. [TypeContent(TRequestBody, Encoding, string)](#typecontenttrequestbody-encoding-string)
4. [Configuring the colleration id](#configuring-the-colleration-id)
5. [Contributing](#contributing)

___

## Getting started

Getting started with `IdentityServer4.Contrib.HttpClientService` is rather easy, you only need three things:

 1. Install the nuget package [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService)
 2. Provide the options to request an access token in the `appsettings.json`
 3. Register the service in `Startup.cs`

### It's a nuget package!

Install the [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService) nuget package, using your favorite way.

### IdentityServer4 Access Token Request Options

Add the IdentityServer4 Access Token Request Options to your `appsettings.json` (the configuration section should always be or end with `ClientCredentialsOptions`):

```json
"ClientCredentialsOptions": {
    "Address": "https://demo.identityserver.io/connect/token",
    "ClientId": "m2m",
    "ClientSecret": "secret",
    "Scope": "api"
}
 // The values above are part of the demo offered in https://demo.identityserver.io/
```

> You could very well skip this step if you use one of [the other ways of setting up an Access Token Request](#how-to-setup-an-access-token-request).

### Register the service

Register the service In `StartUp.cs` in `ConfigureServices(IServiceCollection services)`:

```csharp
services.AddHttpClientService();
```

> You would also need to configure the service if you plan to use the [options pattern](#setidentityserveroptionstoptionsioptionstoptions).

### You are done!

Request the `IHttpClientServiceFactory` wherever you want to make the authenticated requests:

```csharp
using IdentityServer4.Contrib.HttpClientService.Extensions;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
	//Request the IHttpClientServiceFactory instance in your controller or service
	private readonly IHttpClientServiceFactory _requestServiceFactory;
	public CustomerController(IHttpClientServiceFactory requestServiceFactory){
		_requestServiceFactory = requestServiceFactory;
	}

	[HttpGet]
	public async Task<IActionResult> Get(){
		//Make the request
		var responseObject = await _requestServiceFactory
			//Create a instance of the service
			.CreateHttpClientService()
			//Supports many ways of setting IdentityServer options
			.SetIdentityServerOptions("ClientCredentialsOptions")
			//GET and deserialize the response body to IEnumerable<Customers>
			.GetAsync<IEnumerable<Customers>>("https://api/customers");

		//Do something with the results					  
		if (!responseObject.HasError)
		{
			var customers = responseObject.BodyAsType;
			return Ok(customers);
		}
		else
		{
			var httpStatusCode = responseObject.StatusCode;
			var errorMessage = responseObject.Error;           
			return StatusCode((int)httpStatusCode, errorMessage);
		}
	}
}	
```

> The `.SetIdentityServerOptions("SomeClientCredentialsOptions")` might be the simplest way of setting up an [Access Token Request](#how-to-setup-an-access-token-request), the [Options Pattern](#setidentityserveroptionstoptionsioptionstoptions) though is the suggested one. Keep in mind, that if you use the `.SetIdentityServerOptions("SomeClientCredentialsOptions")` approach, the configuration section should either be or end with `ClientCredentialsOptions`.
> HTTP verbs supported are: [GET](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html). 

Continue reading for more details!
You can also take a look at the [Documentation](api/index.md) for technical details, check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).

___

## How to setup an Access Token Request

The library supports multiple ways for setting up the necessary options for retrieving an access token. Upon success of retrieving one, the result is cached until the token expires; that means that a new request to a protected resource does not necessarily means a new request for an access token.

> Currently, the library only supports `ClientCredentialsTokenRequest` and `PasswordTokenRequest`.

### .SetIdentityServerOptions(String)

Setup IdentityServer options by defining the configuration section where you have your settings. The type of the options (`ClientCredentialsOptions` or `PasswordOptions`) will be determined based on the name of the section:

```csharp
//...
.SetIdentityServerOptions("SomeClientCredentialsOptions")
//...
```

Although this option is not adviced for `PasswordTokenRequest`, the section can contain the properties of either the `ClientCredentialsOptions` or `PasswordOptions` objects. Keep in mind, that if you use the `.SetIdentityServerOptions("SomeClientCredentialsOptions")` approach, the configuration section should either be or end with `ClientCredentialsOptions`.

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

Setup IdentityServer options using the options pattern (read more about the options pattern in [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)):

**Startup.cs**

```csharp
//...
    public void ConfigureServices(IServiceCollection services)
    {
        //...
        services.AddHttpClientService()
            .AddSingleton<IProtectedResourceService, ProtectedResourceService>()
            .Configure<ClientCredentialsOptions>(Configuration.GetSection(nameof(ClientCredentialsOptions)));    
        //...
    }
//...
```

**ProtectedResourceService.cs**

```csharp
//...
public class ProtectedResourceService : IProtectedResourceService
{
  private readonly IHttpClientServiceFactory _requestServiceFactory;
  private readonly IOptions<ClientCredentialsOptions> _identityServerOptions;

  public ProtectedResourceService(IHttpClientServiceFactory requestServiceFactory, IOptions<ClientCredentialsOptions> identityServerOptions)
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

Responses can always be deserialized to the type `TResponseBody` defined in, for example, `GetAsync<TResponseBody>`:

```csharp
//...
.GetAsync<ResponsePoco>("https://url_that_returns_ResponsePoco_in_json");
//...
```

Using a complex type as a request body for POST, PUT and PATCH requests is also very easy. In the example that follows the type `TRequestBody` of the `PostAsync<TRequestBody,TResponseBody>` sets the type of the `requestPoco` object. This will be serialized using `JsonConvert.SerializeObject(requestPoco, Formatting.None)`:

```csharp
//...
.PostAsync<RequestPoco,ResponsePoco>("https://url_that_accepts_RequestPoco_and_responds_with_ResponsePoco", requestPoco);
//...
```
> If you want to fine tune how the `requestPoco` object is sent, please use the [TypeContent(TRequestBody, Encoding, string)](#typecontenttrequestbody-encoding-string). Without using `TypeContent(...)` to explitily set media-type and encoding, the defaults will be used: 'application/json' and 'UTF-8'.

### ResponseObject

The variable **[responseObject](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html)** contains multiple properties: from the entire `HttpResponseMessage` and `HttpRequestMessage`, to the `HttpStatusCode` and `HttpResponseHeaders`. The most *exciting* feature though, is the `TResponseBody BodyAsType` property which will contain the deserializabled complex types from JSON responses. For a complete list of all the properties, check the [ResponseObject&lt;TResponseBody&gt;](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html) in the docs.

### TypeContent(TRequestBody, Encoding, string)
You can also fine tune encoding and media-type by using the `TypeContent(TRequestBody model, Encoding encoding, string mediaType)` like this:

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          .CreateHttpClientService()
                          //.PostAsync<TRequestBody,TResponseBody>(URL, customer of type Customer1)
                          .PostAsync<TypeContent<Customer1>,Customer2>("https://api/customers", new TypeContent(customer, Encoding.UTF8, "application/json"));
```

___

### Configuring the colleration id
Starting from version 2.3, the colleration id used to for logging between cascading API calls, can be configured from appsettings using the options pattern:

**appsettings.json**

```csharp
"HttpClientServiceOptions": {
	//Switches on or off the sychronization of the colleration id
	"HeaderCollerationIdActive": true,
	//Sets the name of the header
	"HeaderCollerationName": "X-Request-ID"
},
 // The values above are part of the demo offered in https://demo.identityserver.io/
```

**Startup.cs**

```csharp
//...
    public void ConfigureServices(IServiceCollection services)
    {
        //...
        services.AddHttpClientService()
		.Configure<HttpClientServiceOptions>(Configuration.GetSection(nameof(HttpClientServiceOptions))); 
        //...
    }
//...
```
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
