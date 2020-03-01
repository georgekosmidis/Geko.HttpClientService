using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using System;
using System.Linq;

namespace IdentityServer4.Contrib.HttpClientService.FeaturesSample.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("sample-typed-response")]
    public class SampleGetRequestController : ControllerBase
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;

        /// <summary>
        /// Constructor of the <see cref="SampleIdentityServer4AuthController"/>
        /// </summary>
        /// <param name="testApiService"></param>
        public SampleGetRequestController(IHttpClientServiceFactory requestServiceFactory)
        {
            _requestServiceFactory = requestServiceFactory;
        }

        /// <summary>
        /// A simple GET to a resource that returns the response body as string
        /// </summary>
        /// <returns></returns>
        [HttpGet("raw")]
        public async Task<IActionResult> SimpleGet()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues,
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core)
                .GetAsync("https://www.google.com");                                        //Execute the GET request


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                     //If not, get the body of the respone in the BodyAsString property
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// GET request to a resource where the response body is converted to integer
        /// </summary>
        /// <returns></returns>
        [HttpGet("int")]
        public async Task<IActionResult> SimpleGetWithTypeCasting1()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                  //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues,
                                                                                            // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core)
                .GetAsync<int>("http://localhost:5000/dummy-data/random-integer");          //Execute a GET request and set the type of the result to int


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsType / 2);                                   //If not, get the body as type (divides by two sd proof of concept)
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// GET request to a resource where the response body is converted to list of <see cref="ComplexTypeSampleModel"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("complex-type")]
        public async Task<IActionResult> SimpleGetWithTypeCasting2()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                                          //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues,
                                                                                                                    // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core)
                .GetAsync<IEnumerable<ComplexTypeSampleModel>>("http://localhost:5000/dummy-data/complex-type");    //Execute a GET request and set the type of the result to a list of ComplexTypeSampleModel


            if (!responseObject.HasError)                                                                           //Check if there was an error in the process
                return Ok(responseObject.BodyAsType.Select(x => x.Firstname));                                      //If not, get the body as type (selects all firstnames as proof of concept)
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);                            //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// A sample model that represents the response of the http://localhost:5000/dummy-data/complex-type
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
