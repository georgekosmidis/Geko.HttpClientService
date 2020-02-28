using Microsoft.AspNetCore.Mvc.Testing;
using IdentityServer4.HttpClientService.SampleProject;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Xunit;
using System.Linq;

namespace IdentityServer4.HttpClientService.SampleProject.Tests
{
    public class IntegrationTestsControllerTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTestsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SampleController_GetTestApiResults_Parallelism()
        {
            // Arrange
            var client = _factory.CreateClient();

            var actionBlock = new ActionBlock<string>(
                async url =>
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    Assert.Equal("application/json; charset=utf-8",
                       response.Content.Headers.ContentType.ToString());

                    var body = await response.Content.ReadAsStringAsync();

                    Assert.Contains("{\"Key\":\"x-integration-test-header\",\"Value\":[\"" + url.Split('/').Last() + "\"]}", body);

                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });


            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
            {
                await actionBlock.SendAsync("/IntegrationTests/test/request/headers/" + Guid.NewGuid());
            }
            actionBlock.Complete();
            actionBlock.Completion.Wait();
        }
    }
}
