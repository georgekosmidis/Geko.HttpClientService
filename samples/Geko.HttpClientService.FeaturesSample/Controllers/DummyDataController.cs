using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Geko.HttpClientService.FeaturesSample.Controllers;

/// <summary>
/// Sample controller for the <see cref="HttpClientService"/>
/// </summary>
[ApiController]
[Route("dummy-data")]
public class DummyDataController : ControllerBase
{

    /// <summary>
    /// Constructor of the <see cref="DummyDataController"/>
    /// </summary>
    public DummyDataController() { }

    /// <summary>
    /// Returns dummy responses for the rest of the endpoints
    /// </summary>
    /// <returns></returns>
    [HttpGet("complex-type")]
    public IActionResult GetDummyResults()
    {

        return Ok(new[]
        {
                new {
                    Id = new Random().Next(int.MinValue, int.MaxValue),
                    Firstname = "George",
                    LastName = "Kosmidis",
                    Date = DateTime.Now
                },
                new {
                    Id = new Random().Next(int.MinValue, int.MaxValue),
                    Firstname = "Jota",
                    LastName = "Vekiari",
                    Date = DateTime.Now
                }
            });
    }

    /// <summary>
    /// Returns dummy responses for the rest of the endpoints
    /// </summary>
    /// <returns></returns>
    [HttpPost("complex-type")]
    public IActionResult PostDummyResults([FromBody] ComplexTypeSampleModel sampleModel)
    {
        return Created("http://localhost/5000/complex-type", sampleModel);
    }
    /// <summary>
    /// A sample object representing the body of the request
    /// </summary>
    public class ComplexTypeSampleModel
    {
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Date { get; set; }
    }

    /// <summary>
    /// Returns a random integer
    /// </summary>
    /// <returns></returns>
    [HttpGet("random-integer")]
    public IActionResult GetRandomInteger()
    {
        return Ok(new Random().Next(int.MinValue, int.MaxValue));
    }

    /// <summary>
    /// Accepts an integer and writes that to the body
    /// </summary>
    /// <returns></returns>
    [HttpPost("post-integer")]
    public async Task<IActionResult> PostInteger()
    {
        //Unfortunately ASP.NET Core doesn't let you just capture 'raw' data in any meaningful way just by way of method parameters. 
        // Read more: https://stackoverflow.com/questions/58911465/unsupported-media-type-when-consuming-text-plain
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        return Ok(
            await reader.ReadToEndAsync()
        );
    }
}
