using Geko.HttpClientService.Extensions;
using Geko.HttpClientService.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Test;

[TestClass]
public class HttpClientServiceHeadExtensionsTests
{

    [TestMethod]
    public async Task HttpClientServiceHead_NoTypedResponse_ShouldBeResponseString()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

        var result = await httpClientService.HeadAsync("http://localhost");

        //Status/HttpResponseMessage
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage?.StatusCode);

        //HttpRequestMessage
        Assert.AreEqual(HttpMethod.Head, result.HttpRequestMessge.Method);
        Assert.IsNull(result.HttpRequestMessge.Content);

        //Body
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsType);
    }

    [TestMethod]
    public async Task HttpClientServiceHead_TypedResponse_ShouldBeResponseType()
    {
        var httpClientService = await Tests.Helpers.HttpClientServiceInstances.GetNew(HttpStatusCode.OK, ComplexTypes.ComplexTypeResponseString, true);

        var result = await httpClientService.HeadAsync<ComplexTypes.ComplexTypeResponse>("http://localhost");

        //Status/HttpResponseMessage
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage?.StatusCode);

        //HttpRequestMessage
        Assert.AreEqual(HttpMethod.Head, result.HttpRequestMessge.Method);
        Assert.IsNull(result.HttpRequestMessge.Content);

        //Body
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseString, result.BodyAsString);

        Assert.IsInstanceOfType(result.BodyAsType.TestInt, typeof(int));
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestInt, result.BodyAsType.TestInt);

        Assert.IsInstanceOfType(result.BodyAsType.TestBool, typeof(bool));
        Assert.AreEqual(ComplexTypes.ComplexTypeResponseInstance.TestBool, result.BodyAsType.TestBool);
    }

}
