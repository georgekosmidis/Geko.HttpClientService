using IdentityServer4.Contrib.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services
{
    /// <summary>
    /// Abstraction for a test service that accesses a protected resource
    /// </summary>
    public interface IPasswordProtectedResourceService
    {
        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PasswordProtectedResourceResponseDto}"/>. </returns>
        Task<IEnumerable<PasswordProtectedResourceResponseDto>> GetProtectedResourceResults(string username, string password);
    }
}