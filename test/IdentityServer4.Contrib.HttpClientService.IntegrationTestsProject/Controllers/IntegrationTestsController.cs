using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Contrib.HttpClientService.FeaturesSample.ProtectedResourceServices.Dto;
using IdentityServer4.Contrib.HttpClientService.FeaturesSample.ProtectedResourceServices.Services;

namespace IdentityServer4.Contrib.HttpClientService.FeaturesSample.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class IntegrationTestsController : ControllerBase
    {
        private ProtectedResourceService _protectedResourceService;

        /// <summary>
        /// Constructor of the <see cref="IntegrationTestsController"/>
        /// </summary>
        /// <param name="protectedResourceService"></param>
        public IntegrationTestsController(ProtectedResourceService protectedResourceService)
        {
            _protectedResourceService = protectedResourceService;
        }


        /// <summary>
        /// Performs a GET request using a custom <see cref="ProtectedResourceService"/>.
        /// </summary>
        /// <returns>An ActionResult with the typed result returned by the <see cref="ProtectedResourceService"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProtectedResourceResponseDto>>> Get()
        {
            var result = await _protectedResourceService.GetProtectedResourceResults();

            return Ok(result);
        }

        /// <summary>
        /// Performs a GET request using a custom <see cref="ProtectedResourceService"/> and passing a custom header.
        /// </summary>
        /// <returns>An ActionResult with the request headersfor testing purposes</returns>

        [HttpGet("test/request/headers/{header}")]
        public async Task<IActionResult> TestRequestHeaders(string header)
        {
            var result = await _protectedResourceService.GetProtectedResourceResponseObject(
                new Dictionary<string, string>
                {
                    { "x-integration-test-header", header }
                }
            );

            return Ok(result.HttpRequestMessge.Headers);
        }

    }
}
