namespace NotesService.API.Abstractions.DTO.Auth;

public record UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}