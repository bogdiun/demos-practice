namespace NotesService.API.DTO;

public record NotePutRequest
{
    public string NoteValue { get; set; }

    public IEnumerable<string> Categories { get; set; }

    public string NoteKey { get; set; }
}
