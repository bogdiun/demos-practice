namespace NotesService.API.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotesService.API.Abstractions.DTO.Auth;
using NotesService.API.Auth;

// TODO: eventually should be some external api / identity server

[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IValidator<UserLoginRequest> _userLoginValidator;
    private readonly IValidator<UserRegistrationRequest> _userRegistrationValidator;

    public AuthController(
        IAuthenticationService authService,
        IValidator<UserLoginRequest> userLoginValidator,
        IValidator<UserRegistrationRequest> userRegistrationValidator)
    {
        _authService = authService;
        _userLoginValidator = userLoginValidator;
        _userRegistrationValidator = userRegistrationValidator;
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var validationResult = _userLoginValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

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
        var validationResult = _userRegistrationValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

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