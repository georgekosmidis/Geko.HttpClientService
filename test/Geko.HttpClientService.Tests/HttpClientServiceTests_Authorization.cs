using Geko.HttpClientService.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Test;

[TestClass]
public class HttpClientServiceTests_Authorization
{

    [TestMethod]
    public async Task HttpClientServiceTests_Authorization_NoIdentityServerOptions_ShouldRequestWitnNoAuth()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

        var result = await httpClientService.SendAsync<object, string>(
            new Uri("http://localhost"),
            HttpMethod.Get,
            null
        );

        Assert.IsFalse(result.HttpRequestMessge.Headers.Contains("Authorization"));
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual("body_of_response", result.BodyAsType);
    }

    [TestMethod]
    public async Task HttpClientServiceTests_Authorization_WithIdentityServerOptions_ShouldRequestWitnAuth()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

        httpClientService.SetIdentityServerOptions(
            Options.Create(
                new ClientCredentialsOptions
                {
                    Address = "http://localhost",
                    ClientId = "ClientId",
                    ClientSecret = "ClientSecret",
                    Scope = "scope"
                }
            )
        );

        var result = await httpClientService.SendAsync<object, string>(
            new Uri("http://localhost"),
            HttpMethod.Get,
            null
        );

        result.HttpRequestMessge.Dispose();

        Assert.IsTrue(result.HttpRequestMessge.Headers.Contains("Authorization"));
        Assert.AreEqual("Bearer", result.HttpRequestMessge.Headers?.Authorization?.Scheme);
        Assert.AreEqual("access_token", result.HttpRequestMessge.Headers?.Authorization?.Parameter);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual("body_of_response", result.BodyAsType);
    }

    [TestMethod]
    public async Task HttpClientServiceTests_Authorization_WithWrongIdentityServerOptions_ShouldHaveError()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", false);

        httpClientService.SetIdentityServerOptions(
            Options.Create(
                new ClientCredentialsOptions
                {
                    Address = "http://localhost",
                    ClientId = "ClientId",
                    ClientSecret = "ClientSecret",
                    Scope = "scope"
                }
            )
        );

        var result = await httpClientService.SendAsync<string, object>(
            new Uri("http://localhost"),
            HttpMethod.Get,
            null
        );

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsTrue(result.HasError);
        Assert.AreEqual("invalid_client", result.Error.Trim());
    }

}
