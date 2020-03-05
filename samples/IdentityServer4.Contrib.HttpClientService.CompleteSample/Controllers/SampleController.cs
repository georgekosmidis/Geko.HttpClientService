using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IProtectedResourceService _protectedResourceService;

        /// <summary>
        /// Constructor of the <see cref="SampleController"/>
        /// </summary>
        /// <param name="protectedResourceService"></param>
        public SampleController(IProtectedResourceService protectedResourceService)
        {
            _protectedResourceService = protectedResourceService;
        }

        /// <summary>
        /// Sample on how to use a custom service to get typed data from a protected resource
        /// </summary>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _protectedResourceService.GetProtectedResourceResults());
        }
    }
}
