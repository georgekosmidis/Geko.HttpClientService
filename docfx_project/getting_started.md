## Getting started

Getting started with `Geko.HttpClientService` is rather easy, you only need three things:

 1. Install the nuget package [Geko.HttpClientService](https://www.nuget.org/packages/Geko.HttpClientService)
 2. Optionally, provide the options to request an access token in the `appsettings.json`
 3. Register the service in `Startup.cs`

### It's a nuget package!

Install the [Geko.HttpClientService](https://www.nuget.org/packages/Geko.HttpClientService) nuget package, using any of your favorite ways.

### Optionally, set the IdentityServer4 Access Token Request Options

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

### Register the service

Register the service In `StartUp.cs` in `ConfigureServices(IServiceCollection services)`:

```csharp
services.AddHttpClientService()
        //Optional, set it if you have ClientCredentialsOptions or PasswordOptions
        .Configure<ClientCredentialsOptions>(Configuration.GetSection(nameof(ClientCredentialsOptions)));
```

### You are done

Request the `IHttpClientServiceFactory` wherever you want to make the authenticated requests:

```csharp
using Geko.HttpClientService.Extensions;

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

> Configuring the service from startup following the [Options Pattern](#setidentityserveroptionstoptionsioptionstoptions) is the simpler way, but there are [more ways](#how-to-setup-an-access-token-request)
> HTTP verbs supported are: [GET](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html).