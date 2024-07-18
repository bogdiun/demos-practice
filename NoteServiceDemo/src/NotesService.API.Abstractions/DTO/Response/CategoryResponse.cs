namespace NotesService.API.Abstractions.DTO.Response;

public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
}