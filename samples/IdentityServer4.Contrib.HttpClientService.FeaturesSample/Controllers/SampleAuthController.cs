using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Contrib.HttpClientService.Extensions;


namespace IdentityServer4.Contrib.HttpClientService.FeaturesSample.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("sample-auth")]
    public class SampleAuthController : ControllerBase
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;

        /// <summary>
        /// Constructor of the <see cref="SampleAuthController"/>
        /// </summary>
        /// <param name="testApiService"></param>
        public SampleAuthController(IHttpClientServiceFactory requestServiceFactory)
        {
            _requestServiceFactory = requestServiceFactory;
        }

        /// <summary>
        /// The simples possible call to a resource that needs no authentication
        /// </summary>
        /// <returns></returns>
        [HttpGet("no-auth")]
        public async Task<IActionResult> NoAuthenticationNeeded()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .GetAsync("http://localhost:5000/dummy-data/complex-type");                 //Execute a GET request to any URL.
                                                                            

            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                     //If not, just return the body
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// Call to an IdentityServer4 protected resource
        /// </summary>
        /// <returns></returns>
        [HttpGet("auth")]
        public async Task<IActionResult> AuthenticationNeeded()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .SetIdentityServerOptions("ProtectedResourceAccessTokenOptions")            //Set the configuration section name that contain the credentials for IdentityServer4
                                                                                            // for typed configuration, check the CompleteSample
                .GetAsync("https://demo.identityserver.io/api/test");                       //Execute a GET request to a protected resource.


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                     //If not, just return the body
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

    }
}
