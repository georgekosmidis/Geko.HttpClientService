using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Contrib.HttpClientService.CompleteSample.ProtectedResourceServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Contrib.HttpClientService.CompleteSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly IProtectedResourceService _protectedResourceService;

        public SampleController(ILogger<SampleController> logger, IProtectedResourceService protectedResourceService)
        {
            _logger = logger;
            _protectedResourceService = protectedResourceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _protectedResourceService.GetProtectedResourceResults());
        }
    }
}
