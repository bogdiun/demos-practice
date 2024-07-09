namespace NotesService.API.DataAccess;

internal record Note : INoteable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } // Word, Text,  Audio, Image, ??
    public IEnumerable<string> Categories { get; set; } // OR GROUPING/ TAGS / ETC. SHOULD PROBABLY BE A SEPARATE MODEL WITH IDS

    //public DateTime Revised { get; set; }

    //public string LearnedStatus { get; set; }
}
