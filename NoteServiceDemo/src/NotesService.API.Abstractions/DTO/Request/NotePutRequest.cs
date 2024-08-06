namespace NotesService.API.Abstractions.DTO.Request;

public record NotePutRequest
{
    public string NoteValue { get; init; }

    public IEnumerable<int> CategoryIds { get; init; }

    public int MediaTypeId { get; init; }
}
