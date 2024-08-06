namespace NotesService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using NotesService.API.Abstractions.DTO.Auth;
using NotesService.API.Auth;

// TODO: eventually should be some external api / identity server

[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        AuthenticationResult authResponse = await _authService.LoginAsync(request.Email, request.Password);

        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailureResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
        });
    }

    [HttpPost]
    [Route("auth/register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        // TODO: add validation of the UserRegistrationRequest

        AuthenticationResult authResponse = await _authService.RegisterAsync(request.Email, request.Password);

        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailureResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
        });
    }

    // TODO: jwt token refresh
}