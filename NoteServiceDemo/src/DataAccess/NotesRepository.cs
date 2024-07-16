namespace NotesService.API.DataAccess;

using Microsoft.EntityFrameworkCore;
using NotesService.API;
using NotesService.API.DataAccess.Models;
using NotesService.API.DTO;

public class NotesRepository(DataContext dbContext) : INotesRepository
{
    public async Task<IList<NoteResponse>> GetAsync(string? mediaType, string? category)
    {
        return await dbContext.Notes.Select(note => new NoteResponse
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries),
            MediaType = note.MediaType,
        }).ToListAsync();
    }

    public async Task<NoteResponse> GetByIdAsync(int id)
    {
        return await dbContext.Notes.Select(note => new NoteResponse()
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries),
            MediaType = note.MediaType,
        }).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<NoteResponse> AddAsync(NotePostRequest notePost)
    {
        Note note = new()
        {
            NoteKey = notePost.NoteKey,
            NoteValue = notePost.NoteValue,
            Categories = string.Join(',', notePost.Categories),
            MediaType = notePost.MediaType,
            LastAccess = DateTime.Now,
        };
        // TODO: assure datetime invariance

        _ = await dbContext.Notes.AddAsync(note);

        var created = await dbContext.SaveChangesAsync();

        if (created <= 0)
        {
            return null;
        }

        return new NoteResponse()
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries),
            MediaType = note.MediaType,
        };
    }

    public async Task<bool> UpdateAsync(int id, NotePutRequest notePut)
    {
        Note note = new()
        {
            Id = id,
            NoteKey = notePut.NoteKey,
            NoteValue = notePut.NoteValue,
            Categories = string.Join(',', notePut.Categories),
            MediaType = notePut.MediaType,
            LastAccess = DateTime.Now,
        };
        // TODO: assure datetime invariance
        // TODO: allow to change only some values, also maybe some things should not be modifyiable (this way it is basically post)

        _ = dbContext.Notes.Update(note);

        var updated = await dbContext.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var note = await dbContext.Notes.FindAsync(id);

        if (note == null)
        {
            return false;
        }

        _ = dbContext.Remove(note);

        var removed = await dbContext.SaveChangesAsync();

        return removed > 0;
    }
}
