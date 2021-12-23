using System.Text;
using Geko.HttpClientService.Extensions;
using Geko.HttpClientService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Geko.HttpClientService.FeaturesSample.Controllers;

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
            .CreateHttpClientService()                                                  //Create a new instance of the HttpClientService
            .HeadersAdd("X-Custom-Header", "Header-Value")                               //Add one header at a time 
            .GetAsync("http://localhost:5000/dummy-data/random-integer");               //Request


        if (!responseObject.HasError)                                                   //Check if there was an error in the process
        {
            return Ok(responseObject.BodyAsType);                                       //If not, get the body as type and divide by two
        }
        else
        {
            return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }
    }

    /// <summary>
    /// Requets with Content-Type headers
    /// </summary>
    /// <returns></returns>
    [HttpGet("content-type")]
    public async Task<IActionResult> ContentHeader()
    {

        var responseObject = await _requestServiceFactory
            .CreateHttpClientService()                                                  //Create a new instance of the HttpClientService
            .PostAsync<string>(                                                         //Execute a POST request, retrieve results as string
                "http://localhost:5000/dummy-data/complex-type",                        //URL for the request
                new StringContent("request_content", Encoding.UTF8, "text/plain")       //StringContent with encoding and mediatype
             );

        if (!responseObject.HasError)                                                   //Check if there was an error in the process
        {
            return Ok(responseObject.BodyAsString);                                     //If not, get the body as type and divide by two
        }
        else
        {
            return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }
    }

    /// <summary>
    /// Requets with Content-Type headers
    /// </summary>
    /// <returns></returns>
    [HttpGet("content-type-2")]
    public async Task<IActionResult> ContentHeader2()
    {

        var responseObject = await _requestServiceFactory
            .CreateHttpClientService()                                                  //Create a new instance of the HttpClientService
            .PostAsync<TypeContent<ComplexTypeSampleModel>, string>(                    //Execute a POST request, retrieve results as string
                "http://localhost:5000/dummy-data/complex-type",                        //URL for the request
                new TypeContent<ComplexTypeSampleModel>(new ComplexTypeSampleModel(), Encoding.UTF8, "application/json")       //StringContent with encoding and mediatype
             );

        if (!responseObject.HasError)                                                   //Check if there was an error in the process
        {
            return Ok(responseObject.BodyAsString);                                     //If not, get the body as type and divide by two
        }
        else
        {
            return StatusCode((int)responseObject.StatusCode, responseObject.Error);    //If an error is found, return the status code and the error description
        }
    }

    /// <summary>
    /// A sample model for the POST request to the http://localhost:5000/dummy-data/complex-type
    /// </summary>
    private class ComplexTypeSampleModel
    {
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Date { get; set; }
    }
}
