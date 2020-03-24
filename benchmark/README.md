# Benchmarking

This is a simple comparison of the native implementation (with IdentityModel) vs the HttpClientService implementation.

A sample of 100 requests where made to the protected resource https://demo.identityserver.io/api/test using client credentials, with an access token taken from the demo token service (https://demo.identityserver.io/connect/token). The results showed that the HttpClientService is 60% faster, due to the [TokenResponseCacheManager](https://georgekosmidis.github.io/IdentityServer4.Contrib.HttpClientService/api/IdentityServer4.Contrib.HttpClientService.Infrastructure.TokenResponseCacheManager.html).


![Benchmarking](https://raw.githubusercontent.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/master/benchmark/benchmark_v2.3.0.png)
*The HttpClientService is **60% to 70% faster** because of the caching that implements.*

> The  [IdentityServer4.Contrib.HttpClientService.Benchmark](https://github.com/georgekosmidis/IdentityServer4.Contrib.HttpClientService/tree/master/benchmark/IdentityServer4.Contrib.HttpClientService.Benchmark) and this file need more work!
