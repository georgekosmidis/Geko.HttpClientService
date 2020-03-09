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

## Getting Started

Getting started with IdentityServer4.Contrib.HttpClientService is easy; you only need three things:

1. Install the nuget package [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService)
2. Provide the options to request an access token in the `appsettings.json`
3. Register the service in `Startup.cs`

Read the entire **[Getting Started](getting_started.md)** guide for a quick start. You can also take a look at the [Documentation](api/index.md), check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).

## More Details

Read the '[More Details](more_details.md)' section for a more detailed explanation on some of the features of this library, like the `SetIdentityServerOptions` overloads, the `ResponseObject` and the `TypeContent(TRequestBody, Encoding, string)` class

## Contributing

Feedback and contibution is more than welcome, as there are many more things to do! 

Just as a sample:
1. Expand the [IdentityServer4.Contrib.HttpClientService.CompleteSample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample) with more functionality.
2. Support `JsonSerializerSettings` for ` JsonConvert.DeserializeObject<TResponseBody>(apiResponse.BodyAsString)` in [HttpClientService]( https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/blob/86262f016173bafd2c9ec4fbe70ac9eb1406042a/src/IdentityServer4.Contrib.HttpClientService/HttpClientService.cs).
3. Support more than `ClientCredentials`.
4. Add logging.

Many more are coming soon and all of them should be [issues](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/issues), so feel free to open one and let's start discussing solutions!