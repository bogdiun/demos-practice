namespace NotesService.API.DataAccess;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotesService.API;
using NotesService.API.DataAccess.Models;
using NotesService.API.DTO;

public class NotesRepository : INotesRepository
{
    private readonly DataContext _dbContext;

    public NotesRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<NoteResponse>> GetAsync(string? mediaType, string? category)
    {
        // TODO implement pagination, cancellation
        return await _dbContext.Notes.Select(note => new NoteResponse
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Select(x => x.Name).ToList(),
            MediaType = note.MediaType.TypeName,
        }).ToListAsync();
    }

    public async Task<NoteResponse> GetByIdAsync(int id)
    {
        return await _dbContext.Notes.Select(note => new NoteResponse()
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Select(x => x.Name).ToList(),
            MediaType = note.MediaType.TypeName,
        }).SingleOrDefaultAsync(x => x.Id == id);
    }

    // TODO: have a controller for categories and mediatypes (add and get all), so that the API user gets them and uses id's to set, search by ID is going to be better
    // TODO: also figure out how to bypass looking up tables before adding new note (like explicit properties for foreign keys)
    public async Task<NoteResponse> AddAsync(NotePostRequest notePost)
    {
        // TODO: rename to plural (by creating without props it made singular
        var addedNote = await _dbContext.Notes.AddAsync(new()
        {
            NoteKey = notePost.NoteKey,
            NoteValue = notePost.NoteValue,
            Categories = await _dbContext.Categories.Where(c => notePost.Categories.Contains(c.Name)).ToListAsync(),
            MediaType = await _dbContext.MediaTypes.SingleAsync(m => m.TypeName.Equals(notePost.MediaType)),
            LastAccess = DateTime.Now,
        });

        var created = await _dbContext.SaveChangesAsync();

        if (created <= 0)
        {
            return null;
        }

        return new NoteResponse()
        {
            Id = addedNote.Entity.Id,
            NoteKey = addedNote.Entity.NoteKey,
            NoteValue = addedNote.Entity.NoteValue,
            Categories = addedNote.Entity.Categories.Select(x => x.Name).ToList(),
            MediaType = addedNote.Entity.MediaType.TypeName,
        };
    }

    // TODO: make sure it is transactional and can be rolled back
    // Might need to redo later to make it more efficient (storedprocedures?)
    public async Task<bool> UpdateAsync(int id, NotePutRequest notePut)
    {
        // TODO something like if categories updated then foreach category updated, add notes(noteids?)
        // for everything else that is updated - go on and update properties on Note

        // In SQL case I would need to insert mediatypeId directly with note and there would be an n amount of insertions to CategoryNotes table)
        var note = await _dbContext.Notes.Include(n => n.MediaType)
                                         .Include(n => n.Categories)
                                         .FirstOrDefaultAsync(n => n.Id == id);

        if (note == null)
        {
            return false;
        }

        // TODO: change dto to specificly state only categories that are added, mediatype change or value change everything else should be same
        note.NoteValue = notePut.NoteValue;

        // TODO: Change put/post to use IDs 
        if (note.MediaType.TypeName != notePut.MediaType)
        {
            note.MediaType = await _dbContext.MediaTypes.SingleAsync(m => m.TypeName.Equals(notePut.MediaType));
        }

        await UpdateCategoriesAsync(note, notePut);

        // TODO: add check if no changes made then don't save 
        int updated = await _dbContext.SaveChangesAsync();

        return updated > 0;
    }

    private async Task UpdateCategoriesAsync(Note note, NotePutRequest notePut)
    {
        var currentIds = note.Categories.Select(c => c.Id).ToList();
        var categoriesToRemove = note.Categories.Where(c => !notePut.Categories.Contains(c.Name)).ToList();
        var addedCategories = await _dbContext.Categories.Where(c => notePut.Categories.Contains(c.Name)
                                                                   && !currentIds.Contains(c.Id))
                                                       .ToListAsync();

        foreach (var category in categoriesToRemove)
        {
            note.Categories.Remove(category);
        }

        foreach (var category in addedCategories)
        {
            note.Categories.Add(category);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var removed = await _dbContext.Notes.Where(n => n.Id == id)
                                         .ExecuteDeleteAsync();
        return removed > 0;
    }
}
