# An HttpClient service for IdentityServer4
[![Build Status](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_apis/build/status/georgekosmidis.IdentityServer4.Contrib.HttpClientService?branchName=master)](https://dev.azure.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/_build/latest?definitionId=5&branchName=master) ![Nuget](https://img.shields.io/nuget/v/IdentityServer4.Contrib.HttpClientService)

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


The variable **[responseObject](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html)** contains multiple properties: from the entire `HttpResponseMessage` and `HttpRequestMessage`, to `HttpStatusCode` and `HttpResponseHeaders`. The most *exciting* feature though, is the `TResponseBody BodyAsType` property which will contain deserializabled complex types from JSON responses. (in the example above `IEnumerable<Customers>`). Check the [ResponseObject&lt;TResponseBody&gt;](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Models.ResponseObject-1.html) in the docs.

HTTP verbs supported are: [GET](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceGetExtensions.html), [POST](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePostExtensions.html), [PUT](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePutExtensions.html), [DELETE](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceDeleteExtensions.html), [PATCH](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServicePatchExtensions.html) and [HEAD](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Extensions.HttpClientServiceHeadExtensions.html). Read the [Documentation](api/index.md) for technical details, check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).

## Getting Started
Getting started with IdentityServer4.Contrib.HttpClientService is easy; you only need three things:
1. Install the nuget package [IdentityServer4.Contrib.HttpClientService](https://www.nuget.org/packages/IdentityServer4.Contrib.HttpClientService)
2. Provide the options to authenticate in `appsettings.json`
3. Register the service in `Startup.cs`

Read the entire **[Getting Started](getting_started.md)** guide for a quick start. You can also take a look at the [Documentation](api/index.md), check the [features sample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.FeaturesSample) or a more [complete one](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample).

## Contributing
Feedback and contibution is more than welcome, as there are many more things to do! 

Just as a sample:
1. Expand the [IdentityServer4.Contrib.HttpClientService.CompleteSample](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/samples/IdentityServer4.Contrib.HttpClientService.CompleteSample) with more functionality.
2. Support `JsonSerializerSettings` for ` JsonConvert.DeserializeObject<TResponseBody>(apiResponse.BodyAsString)` in [HttpClientService]( https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/blob/86262f016173bafd2c9ec4fbe70ac9eb1406042a/src/IdentityServer4.Contrib.HttpClientService/HttpClientService.cs#L300).
3. Remove hard coded values for `StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");` in [HttpClientService]( https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/blob/86262f016173bafd2c9ec4fbe70ac9eb1406042a/src/IdentityServer4.Contrib.HttpClientService/HttpClientService.cs#L252).
4. Support more than `ClientCredentials`.
5. Add logging.

Many more are coming soon and all of them should be [issues](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/issues), so feel free to open one and let's start discussing solutions!