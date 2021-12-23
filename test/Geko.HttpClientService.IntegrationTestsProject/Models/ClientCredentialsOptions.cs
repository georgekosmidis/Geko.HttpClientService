namespace Geko.HttpClientService.IntegrationTestsProject.Models;

public record class ClientCredentialsOptions
{
    public string? Address { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scope { get; set; }

}
