using IdentityServer4.Contrib.HttpClientService.Infrastructure;
using IdentityServer4.Contrib.HttpClientService.Models;
using IdentityServer4.Contrib.HttpClientService.Tests.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace IdentityServer4.Contrib.HttpClientService.Tests.Ifrastracture
{
    [TestClass]
    public class AccessTokenOptionsTests
    {

        [TestMethod]
        public void AccessTokenOptions_NewInstance_ShouldBeClean()
        {
            var accessTokenOptions = new IdentityServerOptionsHandler(
                IConfigurationMocks.Get("key", "section_data")
            );

            Assert.IsFalse(accessTokenOptions.HasOptions);
            Assert.IsNull(accessTokenOptions.Get());
        }

        [TestMethod]
        public void AccessTokenOptions_SetIOptions_ShouldSetOptions()
        {
            var accessTokenOptions = new IdentityServerOptionsHandler(
                IConfigurationMocks.Get("key", "section_data")
            );

            accessTokenOptions.Set(
                Options.Create(
                    new ClientCredentialOptions
                    {
                        Address = "address",
                        ClientId = "clientid",
                        ClientSecret = "clientsecret",
                        Scope = "scope"
                    }
                )
            );

            Assert.IsTrue(accessTokenOptions.HasOptions);
            Assert.IsNotNull(accessTokenOptions.Get());

            Assert.AreEqual("address", accessTokenOptions.Get().Address);
            Assert.AreEqual("clientid", accessTokenOptions.Get().ClientId);
            Assert.AreEqual("clientsecret", accessTokenOptions.Get().ClientSecret);
            Assert.AreEqual("scope", accessTokenOptions.Get().Scope);
        }

        [TestMethod]
        public void AccessTokenOptions_SetDelegage_ShouldSetOptions()
        {
            var accessTokenOptions = new IdentityServerOptionsHandler(
                IConfigurationMocks.Get("key", "section_data")
            );

            accessTokenOptions.Set<ClientCredentialOptions>(
                x =>
                {
                    x.Address = "address";
                    x.ClientId = "clientid";
                    x.ClientSecret = "clientsecret";
                    x.Scope = "scope";
                }
            );

            Assert.IsTrue(accessTokenOptions.HasOptions);
            Assert.IsNotNull(accessTokenOptions.Get());

            Assert.AreEqual("address", accessTokenOptions.Get().Address);
            Assert.AreEqual("clientid", accessTokenOptions.Get().ClientId);
            Assert.AreEqual("clientsecret", accessTokenOptions.Get().ClientSecret);
            Assert.AreEqual("scope", accessTokenOptions.Get().Scope);
        }

        [TestMethod]
        public void AccessTokenOptions_SetObject_ShouldSetOptions()
        {
            var accessTokenOptions = new IdentityServerOptionsHandler(
                IConfigurationMocks.Get("key", "section_data")
            );

            accessTokenOptions.Set(
                new ClientCredentialOptions
                {
                    Address = "address",
                    ClientId = "clientid",
                    ClientSecret = "clientsecret",
                    Scope = "scope"
                }
            );

            Assert.IsTrue(accessTokenOptions.HasOptions);
            Assert.IsNotNull(accessTokenOptions.Get());

            Assert.AreEqual("address", accessTokenOptions.Get().Address);
            Assert.AreEqual("clientid", accessTokenOptions.Get().ClientId);
            Assert.AreEqual("clientsecret", accessTokenOptions.Get().ClientSecret);
            Assert.AreEqual("scope", accessTokenOptions.Get().Scope);
        }

        //todo: moq
        //[TestMethod]
        //public void AccessTokenOptions_SetConfSection_ShouldSetOptions()
        //{
        //    var accessTokenOptions = new AccessTokenOptions(
        //        IConfigurationMocks.Get("DefaultClientCredentialOptions", @"
        //            ""DefaultClientCredentialOptions"": {
        //                ""Address"": ""address"",
        //                ""ClientId"": ""clientid"",
        //                ""ClientSecret"": ""clientsecret"",
        //                ""Scope"": ""scope""
        //              }
        //        ")
        //    );

        //    accessTokenOptions.Set("DefaultClientCredentialOptions");

        //    Assert.IsTrue(accessTokenOptions.HasOptions);
        //    Assert.IsNotNull(accessTokenOptions.Get());

        //    Assert.AreEqual("address", accessTokenOptions.Get().Address);
        //    Assert.AreEqual("clientid", accessTokenOptions.Get().ClientId);
        //    Assert.AreEqual("clientsecret", accessTokenOptions.Get().ClientSecret);
        //    Assert.AreEqual("scope", accessTokenOptions.Get().Scope);
        //}
    }
}
