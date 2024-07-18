namespace NotesService.API.Common.DTO.Request;

// TODO: data validation/constraints
public record NotePutRequest
{
    public string NoteValue { get; init; }

    public IEnumerable<string> Categories { get; init; }

    public string MediaType { get; init; }
}
