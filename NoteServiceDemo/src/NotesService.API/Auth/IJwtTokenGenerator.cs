namespace NotesService.API.Auth;

using Microsoft.AspNetCore.Identity;

// TODO: issue tokens via oauth or something?
public interface IJwtTokenGenerator
{
    // TODO: IdentityUser could be replaced with custom user model if necessary
    string GenerateToken(IdentityUser user);
}
