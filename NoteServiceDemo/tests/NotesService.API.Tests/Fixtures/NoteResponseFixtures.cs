namespace NotesService.Tests.Fixtures;

using System;
using System.Threading.Tasks;
using NotesService.API.Abstractions.DTO.Response;

public static class NoteResponseFixtures
{
    public static List<NoteResponse> GetTestNoteResponses() =>
    [
        new NoteResponse
        {
            Id = 1,
            NoteKey = "key",
            NoteValue = "value",
            MediaType = "text",
            Categories = null,
        },
        new NoteResponse
        {
            Id = 2,
            NoteKey = "zumbado",
            NoteValue = "loco-ES",
            MediaType = "text",
            Categories = [ "slang","spanish","word"],
        }
    ];

    internal static async Task<NoteResponse> GetTestNoteResponse() => new()
    {
        Id = 10,
        NoteKey = "key",
        NoteValue = "value",
        MediaType = "text",
        Categories = null,
    };
}
