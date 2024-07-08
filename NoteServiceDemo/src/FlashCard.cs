namespace NotesService.API;

internal record FlashCard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string LearnedStatus { get; set; }
    public DateTime Revised { get; set; }
    public string Type { get; set; } // Word, Text,  Audio, Image, ??
}
