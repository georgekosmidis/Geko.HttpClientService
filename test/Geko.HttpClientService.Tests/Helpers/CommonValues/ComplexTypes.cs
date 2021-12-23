namespace Geko.HttpClientService.Tests.Helpers;

public static class ComplexTypes
{
    public static string ComplexTypeResponseString { get; } = "{\"testInt\":2,\"testBool\":true}";
    public static ComplexTypeResponse ComplexTypeResponseInstance => new();
    public static ComplexTypeRequest ComplexTypeRequestInstance => new();


    public class ComplexTypeResponse
    {
        public int TestInt { get; set; } = 2;
        public bool TestBool { get; set; } = true;
    }

    public static string ComplexTypeRequestString { get; } = "{\"testInt\":1,\"testBool\":true}";

    public class ComplexTypeRequest
    {
        public int TestInt { get; set; } = 1;
        public bool TestBool { get; set; } = true;
    }
}
