using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Options;
using IdentityServer4.Contrib.HttpClientService.Extensions;

namespace IdentityServer4.Contrib.HttpClientService.IntegrationTestsProject.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class IntegrationTestsController : ControllerBase
    {
        private readonly HttpClientService httpClientService;

        /// <summary>
        /// Constructor of the <see cref="IntegrationTestsController"/>
        /// </summary>
        /// <param name="protectedResourceService"></param>
        public IntegrationTestsController(IHttpClientServiceFactory requestServiceFactory)
        {
            httpClientService = requestServiceFactory
                .CreateHttpClientService()
                .SetIdentityServerOptions("ProtectedResourceClientCredentialsOptions");
        }

        /// <summary>
        /// Outputs the id of the request.
        /// </summary>
        /// <returns>An ActionResult with the typed result returned by the <see cref="ProtectedResourceService"/>.</returns>
        [HttpGet("test/response-from-request/{id}")]
        public ActionResult<Guid> GetResponseFromRequest(Guid id)
        {
            return Ok(id);
        }

        /// <summary>
        /// Performs a GET request to a protected resouse, and adds a custom header.
        /// </summary>
        /// <returns>An ActionResult with the request headersfor testing purposes</returns>

        [HttpGet("test/request/headers/{header}")]
        public async Task<IActionResult> TestRequestHeaders(string header)
        {
            var response = await httpClientService
                .HeadersAdd("x-integration-test-header", header)
                .GetAsync("https://demo.identityserver.io/api/test");
           
            return Ok(response.HttpRequestMessge.Headers);
        }

    }
}
