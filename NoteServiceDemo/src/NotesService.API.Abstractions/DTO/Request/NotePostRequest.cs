namespace NotesService.API.Abstractions.DTO.Request;

public record NotePostRequest
{
    public string NoteKey { get; init; }

    public string NoteValue { get; init; }

    public IEnumerable<int> CategoryIds { get; init; }

    public int MediaTypeId { get; init; }
}