namespace NotesService.API.Abstractions.DTO.Auth;

public record AuthFailureResponse
{
    public IEnumerable<string> Errors { get; set; }
}