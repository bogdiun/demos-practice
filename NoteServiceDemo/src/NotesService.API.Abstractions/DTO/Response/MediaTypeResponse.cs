namespace NotesService.API.Abstractions.DTO.Response;

public record MediaTypeResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
}