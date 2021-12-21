using Geko.HttpClientService.CompleteSample.ClientCredentials2ProtectedResourceServices.Services;
using Geko.HttpClientService.CompleteSample.ClientCredentialsProtectedResourceServices.Services;
using Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Geko.HttpClientService.CompleteSample.Controllers;

/// <summary>
/// Sample controller for the <see cref="HttpClientService"/>
/// </summary>
[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly IClientCredentials2ProtectedResourceService _clientCredentials2ProtectedResourceService;
    private readonly IClientCredentialsProtectedResourceService _clientCredentialsProtectedResourceService;
    private readonly IPasswordProtectedResourceService _passwordProtectedResourceService;

    /// <summary>
    /// Constructor of the <see cref="SampleController"/>
    /// </summary>
    /// <param name="protectedResourceService"></param>
    public SampleController(
        IClientCredentials2ProtectedResourceService clientCredentials2ProtectedResourceService,
        IClientCredentialsProtectedResourceService clientCredentialsProtectedResourceService,
        IPasswordProtectedResourceService passwordProtectedResourceService)
    {
        _clientCredentialsProtectedResourceService = clientCredentialsProtectedResourceService;
        _clientCredentials2ProtectedResourceService = clientCredentials2ProtectedResourceService;
        _passwordProtectedResourceService = passwordProtectedResourceService;
    }

    /// <summary>
    /// Sample on how to use a custom service to get typed data from a protected resource
    /// </summary>
    /// <returns>An <see cref="IActionResult"/></returns>
    [HttpGet("client-credentials")]
    public async Task<IActionResult> GetClientCredentials()
    {
        return Ok(await _clientCredentialsProtectedResourceService.GetProtectedResourceResults());
    }

    [HttpGet("client-credentials-2")]
    public async Task<IActionResult> GetClientCredentials2()
    {
        return Ok(await _clientCredentials2ProtectedResourceService.GetProtectedResourceResults());
    }

    [HttpGet("password")]
    public async Task<IActionResult> GetPassword()
    {
        return await _passwordProtectedResourceService.TrySettingPasswordOptions("username", "password")
            ? Ok(await _passwordProtectedResourceService.GetProtectedResourceResults())
            : BadRequest("Username or password is wrong");
    }
}
