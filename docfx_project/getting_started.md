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
"SomeClientCredentialsOptions": {
    "Address": "https://demo.identityserver.io/connect/token",
    "ClientId": "m2m",
    "ClientSecret": "secret",
    "Scopes": "api"
  }
 // The values above are part of the demo offered in https://demo.identityserver.io/
```

> You could very well skip this step if you use one of [the other ways of setting up an Access Token Request](more_details.md#how-to-setup-an-access-token-request).

### Register the service

Register the service In `StartUp.cs` in `ConfigureServices(IServiceCollection services)`:

```csharp
services.AddHttpClientService();
```
> You would also need to configure the service if you plan to use the [options pattern](more_details.md#setidentityserveroptionstoptionsioptionstoptions).

### You are done!

Request the `IHttpClientServiceFactory` instance wherever you want to make the authenticated requests:

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

> The `.SetIdentityServerOptions("SomeClientCredentialsOptions")` might be the simplest way of setting up an [Access Token Request](more_details.md#how-to-setup-an-access-token-request), the [Options Pattern](more_details.md#setidentityserveroptionstoptionsioptionstoptions) though is the suggested one. 
> HTTP verbs supported are: [GET](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html). 

Continue reading for more details!
You can also take a look at the [Documentation](api/index.md) for technical details, check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).
