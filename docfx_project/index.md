# An easy way to make HTTP requests to JSON endpoints, with IdentityServer4 integration

[![Build Status](https://dev.azure.com/georgekosmidis/Geko.HttpClientService/_apis/build/status/georgekosmidis.Geko.HttpClientService?branchName=master)](https://dev.azure.com/georgekosmidis/Geko.HttpClientService/_build/latest?definitionId=5&branchName=master) ![Nuget](https://img.shields.io/nuget/v/Geko.HttpClientService)

An almost [2x times faster](https://github.com/georgekosmidis/Geko.HttpClientService/tree/master/benchmark) fluent HTTP request service, build to simplify the communication with JSON endpoints, by automatically handling serialization / deserialization. Authenticated requests towards protected by IdentityServer4 resources are done in a simple compact way.

## Add it to the service collection in Startup.cs

```csharp
  services.AddHttpClientService();
  
  //Or if the resources are behind an IdentityServer4
  services.AddHttpClientService(
          .Configure<ClientCredentialsOptions>(Configuration.GetSection(nameof(ClientCredentialsOptions)));
```

## Calls can be easy as that :)

```csharp
var responseObject = await _requestServiceFactory
 //Create a instance of the service
 .CreateHttpClientService()
 //GET and deserialize the response body to IEnumerable<Customers>
 .GetAsync<IEnumerable<Customers>>("https://api/customers");
```

## Getting Started

Getting started with Geko.HttpClientService is easy; you only need three things:

1. Install the nuget package [Geko.HttpClientService](https://www.nuget.org/packages/Geko.HttpClientService)
2. Provide the options to request an access token in the `appsettings.json`
3. Register the service in `Startup.cs`

Read the entire **[Getting Started](getting_started.md)** guide for a quick start. You can also take a look at the [Documentation](api/index.md), check the [features sample](https://github.com/georgekosmidis/Geko.HttpClientService/tree/master/samples/Geko.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/Geko.HttpClientService/tree/master/samples/Geko.HttpClientService.CompleteSample).

## More Details

Read the '[More Details](more_details.md)' section for a more detailed explanation on some of the features of this library, like the `SetIdentityServerOptions` overloads, the `ResponseObject` and the `TypeContent(TRequestBody, Encoding, string)` class

## Contributing

Feedback and contibution is more than welcome, as there are many more things to do!

Just as a sample:

1. Expand the [Geko.HttpClientService.CompleteSample](https://github.com/georgekosmidis/Geko.HttpClientService/tree/master/samples/Geko.HttpClientService.CompleteSample) with more functionality.
2. Support `JsonSerializerSettings` for `JsonConvert.DeserializeObject<TResponseBody>(apiResponse.BodyAsString)` in [HttpClientService]( https://github.com/georgekosmidis/Geko.HttpClientService/blob/86262f016173bafd2c9ec4fbe70ac9eb1406042a/src/Geko.HttpClientService/HttpClientService.cs#L300).
3. Support more than `ClientCredentialsOptions` and `PasswordOptions`.
4. Set options for changing the x-header name
5. Add logging.

Many more are coming soon and all of them should be [issues](https://github.com/georgekosmidis/Geko.HttpClientService/issues), so feel free to open one and let's start discussing solutions!
