namespace Geko.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices.Dto;

/// <summary>
/// An object representing the result of the protected resource https://demo.identityserver.io/api/test
/// </summary>
public record class ClientCredentialsProtectedResourceResponseDto
{
    /// <summary>
    /// The type property of the result
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// The value property of the result
    /// </summary>
    public string? Value { get; init; }
}
