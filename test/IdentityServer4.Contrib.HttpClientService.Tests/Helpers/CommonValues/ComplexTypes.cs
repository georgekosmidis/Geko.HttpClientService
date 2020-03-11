using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public static class ComplexTypes
    {
        public static string ComplexTypeResponseString = "{\"testInt\":2,\"testBool\":true}";
        public static ComplexTypeResponse ComplexTypeResponseInstance => new ComplexTypeResponse();
        public static ComplexTypeRequest ComplexTypeRequestInstance => new ComplexTypeRequest();


        public class ComplexTypeResponse
        {
            public int TestInt { get; set; } = 2;
            public bool TestBool { get; set; } = true;
        }

        public static string ComplexTypeRequestString = "{\"testInt\":1,\"testBool\":true}";

        public class ComplexTypeRequest
        {
            public int TestInt { get; set; } = 1;
            public bool TestBool { get; set; } = true;
        }
    }
}
