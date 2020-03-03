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
    [Route("sample-typed-request")]
    public class SamplePostRequestController : ControllerBase
    {
        private readonly IHttpClientServiceFactory _requestServiceFactory;

        /// <summary>
        /// Constructor of the <see cref="SampleIdentityServer4AuthController"/>
        /// </summary>
        /// <param name="testApiService"></param>
        public SamplePostRequestController(IHttpClientServiceFactory requestServiceFactory)
        {
            _requestServiceFactory = requestServiceFactory;
        }

        /// <summary>
        /// A POST request that sends a string and reads a string from the response body
        /// </summary>
        /// <returns></returns>
        [HttpGet("int")]
        public async Task<IActionResult> SimplePost()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()
                .PostAsync(                                                                 //Execute a POST request, send the body as string, expect the results as string
                    "http://localhost:5000/dummy-data/post-integer", 
                    "request_body"
                );


            if (!responseObject.HasError)                                                   //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                     //If not, get the body as string
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// A POST request that sends an integer and reads a string from the response body
        /// </summary>
        /// <returns></returns>
        [HttpGet("int")]
        public async Task<IActionResult> SimpleTypedPost()
        {
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()
                //.PostAsync<TRequestBody,TResponseBody>(...)
                .PostAsync<int, string>(                                                            //Execute a POST request, sent an integer, expect a string
                    "http://localhost:5000/dummy-data/post-integer",                                //URL for the request
                    19830426                                                                        //Integer to be sent
                );              


            if (!responseObject.HasError)                                                           //Check if there was an error in the process
                return Ok(responseObject.BodyAsString);                                             //If not, get the body as string
            else
                return StatusCode((int)responseObject.StatusCode, responseObject.Error);            //If an error is found, return the status code and the error description
        }

        /// <summary>
        /// A POST request to a resource that accepts a JSON request (a ComplexTypeSampleModel),
        /// and responds with a JSON (represented again by ComplexTypeSampleModel)
        /// </summary>
        /// <returns></returns>
        [HttpGet("complex-type")]
        public async Task<IActionResult> ComplexType()
        {
            
            var responseObject = await _requestServiceFactory
                .CreateHttpClientService()                                                          //Create a new unnamed service (prefer named instances to avoid socket exhaustion issues,
                //.PostAsync<TRequestBody,TResponseBody>(...)                                       // read more about HttpClient issues here: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core)
                .PostAsync<ComplexTypeSampleModel, ComplexTypeSampleModel>(                         //Execute a POST request by setting the type of the RequestBody and ResposeBody to ComplexTypeSampleModel
                    "http://localhost:5000/dummy-data/complex-type",                                //URL for the request
                    new ComplexTypeSampleModel                                                      //Object to be sent
                    {
                        Id = 19830426,
                        Date = DateTime.Now,
                        Firstname = "George",
                        Lastname = "Kosmidis"
                    });    


            if (!responseObject.HasError)                                                           //Check if there was an error in the process
                return Ok(responseObject.BodyAsType.Date);                                          //If not, get the body as type (outputs the Date as proof of concept)
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
