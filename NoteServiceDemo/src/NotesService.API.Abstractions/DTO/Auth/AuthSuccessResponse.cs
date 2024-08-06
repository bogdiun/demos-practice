namespace NotesService.API.Abstractions.DTO.Auth;

public record AuthSuccessResponse
{
    public string Token { get; set; }
}
