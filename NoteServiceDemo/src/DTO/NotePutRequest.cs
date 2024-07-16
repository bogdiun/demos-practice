namespace NotesService.API.DTO;

// TODO: data validation/constraints
public record NotePutRequest
{
    public string NoteKey { get; set; }

    public string NoteValue { get; set; }

    public IEnumerable<string> Categories { get; set; }

    public string MediaType { get; set; }

}
