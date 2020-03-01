using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Tests.Helpers
{
    public static class IHttpContextAccessorMocks
    {
        public static IHttpContextAccessor Get()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockHttpContextAccessor.Object;
        }

        public static IHttpContextAccessor Get(string xHeader)
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Headers["X-HttpClientService"] = xHeader;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockHttpContextAccessor.Object;
        }
    }
}
