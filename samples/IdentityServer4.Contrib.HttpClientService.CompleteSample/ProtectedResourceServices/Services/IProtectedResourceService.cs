using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services
{
    /// <summary>
    /// Abstraction for a test service that accesses a protected resource
    /// </summary>
    public interface IProtectedResourceService
    {
        /// <summary>
        /// Sample request that returns a typed response using GET
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ProtectedResourceResponseDto}"/>. </returns>
        Task<IEnumerable<ProtectedResourceResponseDto>> GetProtectedResourceResults();
    }
}