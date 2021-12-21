﻿namespace Geko.HttpClientService.Models;

/// <summary>
/// Model for the client credentials options for IdentityServer4
/// </summary>
public record class ClientCredentialsOptions : IIdentityServerOptions
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
}
