using IdentityServer4.Contrib.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices.Services
{
    /// <summary>
    /// Abstraction for a test service that accesses a protected resource
    /// </summary>
    public interface IClientCredentialsProtectedResourceService
    {
        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ClientCredentialsProtectedResourceResponseDto}"/>. </returns>
        Task<IEnumerable<ClientCredentialsProtectedResourceResponseDto>> GetProtectedResourceResults();
    }
}