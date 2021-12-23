namespace Geko.HttpClientService.Models;

/// <summary>
/// Model for the password options for IdentityServer4.
/// </summary>
public record class PasswordOptions : IIdentityServerOptions
{
    /// <summary>
    /// The address of the access token service.
    /// (e.g. https://demo.identityserver.io/connect/token)
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// The client id.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// The client secret.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// A space seperated list of scopes.
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// The username of the user trying to get access.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The password of the user trying to get access.
    /// It will be hashed using SHA256 before sending it to the IdentityServer.
    /// </summary>
    public string? Password { get; set; }
}
