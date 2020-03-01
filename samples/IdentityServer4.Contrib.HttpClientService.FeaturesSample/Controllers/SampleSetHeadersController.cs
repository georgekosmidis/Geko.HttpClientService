using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.FeaturesSample.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("sample-headers")]
    public class SampleSetHeadersController : ControllerBase
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;

        /// <summary>
        /// Constructor of the <see cref="SampleIdentityServer4AuthController"/>
        /// </summary>
        /// <param name="testApiService"></param>
        public SampleSetHeadersController(IHttpClientServiceFactory requestServiceFactory)
        {
            _requestServiceFactory = requestServiceFactory;
        }

        /// <summary>
        /// Request with custom headers
        /// </summary>
        /// <returns></returns>
        [HttpGet("custom")]
        public async Task<IActionResult> CustomHeaders()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .AddHeader("X-Custom-Header","Header-Value")                                //Add one header at a time 
                .SetHeaders(new Dictionary<string, string>                                  // or set a dictionary with all headers 
                {
                    { "X-Custom-Header-2", "Header-Value-2" }
                })
                .GetAsync("http://localhost:5000/dummy-data/random-integer");               //Request


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsType);                                       //If not, get the body as type and divide by two
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// Requets with Content-Type headers
        /// </summary>
        /// <returns></returns>
        [HttpGet("content-type")]
        public async Task<IActionResult> ContentHeader()
        {
            
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .PostAsync<string>(                                                         //Execute a POST request, retrieve results as string
                    "http://localhost:5000/dummy-data/complex-type",                        //URL for the request
                    new StringContent("request_content", Encoding.UTF8, "text/plain")       //StringContent with encoding and mediatype
                 );    


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                     //If not, get the body as type and divide by two
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }
        /// <summary>
        /// A sample model for the POST request to the http://localhost:5000/dummy-data/complex-type
        /// </summary>
        private class ComplexTypeSampleModel
        {
            public int Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
