using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.HttpClientService.Extensions;
using System;
using System.Linq;

namespace IdentityServer4.HttpClientService.SampleProject.Controllers
{
    /// <summary>
    /// Sample controller for the <see cref="HttpClientService"/>
    /// </summary>
    [ApiController]
    [Route("sample-typed-request")]
    public class TypedRequestController : ControllerBase
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;

        /// <summary>
        /// Constructor of the <see cref="SampleAuthController"/>
        /// </summary>
        /// <param name="testApiService"></param>
        public TypedRequestController(IHttpClientServiceFactory requestServiceFactory)
        {
            _requestServiceFactory = requestServiceFactory;
        }

        /// <summary>
        /// Call to a resource that accepts an integer in the body and returns an integer as a response body
        /// </summary>
        /// <returns></returns>
        [HttpGet("int")]
        public async Task<IActionResult> SimpleType()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                          //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                                    // Read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .PostAsync<int, int>("http://localhost:5000/dummy-data/post-integer", 19830426);    //Execute a POST request by setting the type of the result to int


            if (!responseObject.HasError)                                                           //Check if there was an error in the process
                return Ok(responseObject.BodyAsType);                                               //If not, get the body as type and divide by two
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);            //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// Call to a resource that accepts a JSON request (a ComplexTypeSampleModel),
        /// and resposds with a JSON (represented again by ComplexTypeSampleModel)
        /// </summary>
        /// <returns></returns>
        [HttpGet("complex-type")]
        public async Task<IActionResult> ComplexType()
        {
            
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                          //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues.
                                                                                                    // Read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
                .PostAsync<ComplexTypeSampleModel, ComplexTypeSampleModel>(                         //Execute a GET request by setting the type of the result to a list of ComplexTypeSampleModel
                    "http://localhost:5000/dummy-data/complex-type",                                //URL for the request
                    new ComplexTypeSampleModel                                                      //Object to be sent
                    {
                        Id = 19830426,
                        Date = DateTime.Now,
                        Firstname = "George",
                        Lastname = "Kosmidis"
                    });    


            if (!responseObject.HasError)                                                           //Check if there was an error in the process
                return Ok(responseObject.BodyAsType.Date);                                          //If not, get the body as type and divide by two
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);            //If an error is found, return the status code and the error description
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
