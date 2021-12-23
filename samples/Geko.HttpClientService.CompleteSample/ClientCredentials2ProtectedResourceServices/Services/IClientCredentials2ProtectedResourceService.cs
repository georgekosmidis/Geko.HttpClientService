using Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Dto;

namespace Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Services;

/// <summary>
/// Abstraction for a test service that accesses a protected resource
/// </summary>
public interface IClientCredentials2ProtectedResourceService
{
    /// <summary>
    /// Sample request that returns a typed response using GET
    /// </summary>
    /// <returns>An <see cref="IEnumerable{OtherClientCredentialsProtectedResourceResponseDto}"/>. </returns>
    Task<IEnumerable<ClientCredentials2ProtectedResourceResponseDto>> GetProtectedResourceResults();
}