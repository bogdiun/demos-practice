namespace NotesService.API.Common.DTO.Request;

public record NotePostRequest
{
    public string NoteKey { get; init; }

    public string NoteValue { get; init; }

    public IEnumerable<string> Categories { get; init; }

    public string MediaType { get; init; }
}