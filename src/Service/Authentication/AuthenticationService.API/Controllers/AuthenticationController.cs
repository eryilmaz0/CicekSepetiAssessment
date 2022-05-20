using AuthenticationService.API.ApiResponses;
using AuthenticationService.API.Models;
using AuthenticationService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class AuthenticationsController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationsController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authenticationService.LoginAsync(request);
        return new(result);
    }


    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok("Auth Service Health Check Success.");
    }
}