using Geko.HttpClientService.Extensions;
using Geko.HttpClientService.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Test;

[TestClass]
public class HttpClientServiceDeleteExtensionsTests
{

    [TestMethod]
    public async Task HttpClientServiceDelete_NoTypedResponse_ShouldBeResponseString()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.NoContent, "", true);
        var result = await httpClientService.DeleteAsync("http://localhost");

        //Status/HttpResponseMessage
        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        Assert.AreEqual(HttpStatusCode.NoContent, result.HttpResponseMessage?.StatusCode);

        //HttpRequestMessage
        Assert.AreEqual(HttpMethod.Delete, result.HttpRequestMessge.Method);
        Assert.IsNull(result.HttpRequestMessge.Content);

        //Body
        Assert.IsTrue(string.IsNullOrEmpty(result.BodyAsString));
        Assert.IsTrue(string.IsNullOrEmpty(result.BodyAsType));
    }

    [TestMethod]
    public async Task HttpClientServiceDelete_TypedResponse_ShouldBeResponseType()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);
        var result = await httpClientService.DeleteAsync<ComplexTypes.ComplexTypeResponse>("http://localhost");

        //Status/HttpResponseMessage
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage?.StatusCode);

        //HttpRequestMessage
        Assert.AreEqual(HttpMethod.Delete, result.HttpRequestMessge.Method);
        Assert.IsNull(result.HttpRequestMessge.Content);

        //Body
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);

        Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestInt, result.BodyAsType.TestInt);

        Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestBool, result.BodyAsType.TestBool);
    }
}
