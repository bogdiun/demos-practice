namespace NotesService.API.Common.DTO.Response;

public record NoteResponse
{
    public int Id { get; init; }

    public string NoteKey { get; init; }

    public string NoteValue { get; init; }

    public IEnumerable<string> Categories { get; init; }

    public string MediaType { get; init; }
}