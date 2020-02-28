using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4.HttpClientService.Tests.Helpers
{
    public static class IConfigurationMocks
    {
        public static IConfiguration Get(string sectionData)
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.IsAny<string>()]).Returns(sectionData);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(mockConfSection.Object);

            return mockConfiguration.Object;
        }


    }
}
