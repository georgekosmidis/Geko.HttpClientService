namespace Geko.HttpClientService.Models;

/// <summary>
/// Settings for the HttpClientService
/// </summary>
public class HttpClientServiceOptions
{
    /// <summary>
    /// A boolean indicating the use or not of the colleration id for logging purposes
    /// </summary>
    public bool HeaderCollerationIdActive { get; set; } = true;

    /// <summary>
    /// The header name of the colleraton id
    /// </summary>
    public string HeaderCollerationName { get; set; } = "X-Request-ID";
}
