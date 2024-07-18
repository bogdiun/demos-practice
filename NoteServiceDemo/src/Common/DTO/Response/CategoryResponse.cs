namespace NotesService.API.Common.DTO.Response;

public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
}