using System.Threading.Tasks.Dataflow;
using Geko.HttpClientService.IntegrationTestsProject;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Geko.HttpClientService.FeaturesSample.Tests;

public class IntegrationTests
    : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public IntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SampleController_GetTestApiResults_Concurrency_ShouldContainCorrectHeader()
    {
        if (Environment.ProcessorCount == 1)
        {
            throw new InvalidOperationException("Concurrency tests with 1 processor are not possible!");
        }

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

                Assert.Contains("\"key\":\"x-integration-test-header\",\"value\":[\"" + url.Split('/').Last() + "\"]", body);

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
