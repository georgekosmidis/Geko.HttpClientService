using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services
{
    public interface IProtectedResourceService
    {
        Task<IEnumerable<ProtectedResourceResponseDto>> GetProtectedResourceResults();
    }
}