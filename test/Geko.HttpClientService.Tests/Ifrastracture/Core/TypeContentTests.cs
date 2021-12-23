using System.Text;
using Geko.HttpClientService.Infrastructure;
using Geko.HttpClientService.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geko.HttpClientService.Tests.Ifrastracture.Core;

[TestClass]
public class TypeContentTests
{
    [TestMethod]
    public async Task TypeContent_ConstructorWithRequestOnly_ShouldAddDefaultHeaders()
    {
        var typeContent = new TypeContent<ComplexTypes.ComplexTypeRequest>(ComplexTypes.ComplexTypeRequestInstance);

        Assert.AreEqual("application/json", typeContent?.Headers?.ContentType?.MediaType);
        Assert.AreEqual("utf-8", typeContent?.Headers?.ContentType?.CharSet);

        var content = await typeContent?.ReadAsStringAsync();
        Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), content.ToLower());
    }

    [TestMethod]
    public async Task TypeContent_ConstructorWithEncodingOnly_ShouldAddCorrectHeaders()
    {
        var typeContent = new TypeContent<ComplexTypes.ComplexTypeRequest>(ComplexTypes.ComplexTypeRequestInstance, Encoding.UTF32);

        Assert.AreEqual("application/json", typeContent?.Headers?.ContentType?.MediaType);
        Assert.AreEqual("utf-32", typeContent?.Headers?.ContentType?.CharSet);

        var content = await (typeContent?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));
        Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), content.ToLower());
    }

    [TestMethod]
    public async Task TypeContent_ConstructorWithEncodingAndMediaType_ShouldAddCorrectHeaders()
    {
        var typeContent = new TypeContent<ComplexTypes.ComplexTypeRequest>(ComplexTypes.ComplexTypeRequestInstance, Encoding.UTF32, "test/type");

        Assert.AreEqual("test/type", typeContent?.Headers?.ContentType?.MediaType);
        Assert.AreEqual("utf-32", typeContent?.Headers?.ContentType?.CharSet);

        var content = await (typeContent?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));
        Assert.AreEqual(ComplexTypes.ComplexTypeRequestString.ToLower(), content.ToLower());
    }
}
