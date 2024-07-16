namespace NotesService.API.DTO;

public record NoteResponse
{
    public int Id { get; set; }

    public string NoteKey { get; set; }

    public string NoteValue { get; set; }

    public IEnumerable<string> Categories { get; set; }

    public string MediaType { get; set; }
}