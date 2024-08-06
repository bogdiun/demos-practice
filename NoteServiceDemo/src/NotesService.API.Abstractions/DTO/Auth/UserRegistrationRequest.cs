namespace NotesService.API.Abstractions.DTO.Auth;

public record UserRegistrationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
