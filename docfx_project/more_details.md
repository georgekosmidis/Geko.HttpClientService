## How to setup an Access Token Request

The library supports multiple ways for setting up the necessary options for retrieving an access token. Upon success of retrieving one, the result is cached until the token expires; that means that a new request to a protected resource does not necessarily means a new request for an access token.

> Currently, the library only supports `ClientCredentialsTokenRequest` and `PasswordTokenRequest`.


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

#### Startup.cs

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

#### ProtectedResourceService.cs

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

Responses can always be deserialized to the type `TResponseBody` with `GetAsync<TResponseBody>`:

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

> If you want to fine tune how the `requestPoco` object is sent, please use the [TypeContent(TRequestBody, Encoding, string)](#typecontenttrequestbody-encoding-string). Without using `TypeContent(...)` to explitily set media-type and encoding, the defaults will be used ('application/json' and 'UTF-8').

### ResponseObject

The variable **[responseObject](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Models.ResponseObject-1.html)** contains multiple properties: from the entire `HttpResponseMessage` and `HttpRequestMessage`, to the `HttpStatusCode` and `HttpResponseHeaders`. The most *exciting* feature though, is the `TResponseBody BodyAsType` property which will contain the deserializabled complex types from JSON responses. For a complete list of all the properties, check the [ResponseObject&lt;TResponseBody&gt;](https://georgekosmidis.github.io/Geko.HttpClientService/api/Geko.HttpClientService.Models.ResponseObject-1.html) in the docs.

### TypeContent(TRequestBody, Encoding, string)

You can also fine tune encoding and media-type by using the `TypeContent(TRequestBody model, Encoding encoding, string mediaType)` like this:

```csharp
var responseObject = await _requestServiceFactory
                          //Create a instance of the service
                          .CreateHttpClientService()
                          //.PostAsync<TRequestBody,TResponseBody>(URL, customer of type Customer1)
                          .PostAsync<TypeContent<Customer1>,ReturnedObject>("https://api/customers", new TypeContent(customer, Encoding.UTF8, "application/json"));
```

___

### Configuring the colleration id

Starting from version 2.3, a colleration id can be used for logging between cascading API calls. It can be configured from appsettings using the options pattern:

#### appsettings.json

```csharp
"HttpClientServiceOptions": {
 //Switches on or off the sychronization of the colleration id
 "HeaderCollerationIdActive": true,
 //Sets the name of the header
 "HeaderCollerationName": "X-Request-ID"
},
```

#### Configuring in Startup.cs for the Header Colleration Id

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