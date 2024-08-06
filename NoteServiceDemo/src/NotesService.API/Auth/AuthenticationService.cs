namespace NotesService.API.Auth;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
                UserManager<IdentityUser> userManager,
                IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser == null)
        {
            return new AuthenticationResult
            {
                Errors = ["User does not exist for this email."],
            };
        }

        var validPassword = await _userManager.CheckPasswordAsync(existingUser, password);

        if (!validPassword)
        {
            return new AuthenticationResult
            {
                Errors = ["User/password is incorrect."],
            };
        }

        string token = _jwtTokenGenerator.GenerateToken(existingUser);

        return new AuthenticationResult
        {
            Success = true,
            Token = token,
        };
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            // TODO: implement remind me password
            return new AuthenticationResult
            {
                Errors = ["User has already been created for this email."],
            };
        }

        IdentityUser newUser = new()
        {
            UserName = email,
            Email = email,
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult
            {
                Errors = createdUser.Errors.Select(e => e.Description),
            };
        }

        string token = _jwtTokenGenerator.GenerateToken(newUser);

        return new AuthenticationResult
        {
            Success = true,
            Token = token,
        };
    }
}
