namespace NotesService.API.Abstractions.DTO.Request;

public record CategoryRequest
{
    public string Name { get; init; }
}