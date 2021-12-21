using Geko.HttpClientService.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Test;

[TestClass]
public class HttpClientServiceTests_SetIdentityOptions_PasswordOptions
{

    [TestMethod]
    public async Task HttpClientServiceTests_HttpClientServiceTests_SetIdentityOptionsIOptions_PasswordOptions_ShouldRequestWitnAuth()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

        httpClientService.SetIdentityServerOptions(
            Options.Create(
                new PasswordOptions
                {
                    Address = "http://localhost",
                    ClientId = "ClientId",
                    ClientSecret = "ClientSecret",
                    Scope = "Scope",
                    Username = "Username",
                    Password = "Password"
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
    public async Task HttpClientServiceTests_HttpClientServiceTests_SetIdentityOptionsObject_PasswordOptions_ShouldRequestWitnAuth()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

        httpClientService.SetIdentityServerOptions(
            new PasswordOptions
            {
                Address = "http://localhost",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                Scope = "Scope",
                Username = "Username",
                Password = "Password"
            }
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
    public async Task HttpClientServiceTests_HttpClientServiceTests_SetIdentityOptionsDelegate_PasswordOptions_ShouldRequestWithAuth()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, "body_of_response", true);

        httpClientService.SetIdentityServerOptions<PasswordOptions>(x =>
           {
               x.Address = "http://localhost";
               x.ClientId = "ClientId";
               x.ClientSecret = "ClientSecret";
               x.Scope = "Scope";
               x.Username = "Username";
               x.Password = "Password";
           }
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
}
