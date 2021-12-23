using Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Dto;

namespace Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services;

/// <summary>
/// Abstraction for a test service that accesses a protected resource
/// </summary>
public interface IPasswordProtectedResourceService
{
    /// <summary>
    /// Sample request that returns a typed response using GET
    /// </summary>
    /// <returns>An <see cref="IEnumerable{PasswordProtectedResourceResponseDto}"/>. </returns>
    Task<IEnumerable<PasswordProtectedResourceResponseDto>> GetProtectedResourceResults();

    /// <summary>
    /// Returns true if an access token is succesfully retrieved
    /// </summary>
    /// <param name="username">The username</param>
    /// <param name="password">The username</param>
    /// <returns>A boolean indicating if the access token has benn succesfully retrieved.</returns>
    Task<bool> TrySettingPasswordOptions(string username, string password);
}
