namespace NotesService.API.DataAccess.Repositories;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;
using NotesService.API.DataAccess.Entities;

internal sealed class NotesRepository : INotesRepository
{
    private readonly DataContext _dbContext;

    public NotesRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    // some Query object?
    public async Task<IList<NoteResponse>> GetAsync(string? userId, int? mediaTypeId, int? categoryId)
    {
        // TODO implement pagination, cancellation, sorting
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }

        IQueryable<Note> notes = _dbContext.Notes.Where(n => n.UserId == userId);

        if (mediaTypeId.HasValue)
        {
            notes = notes.Include(n => n.MediaType)
                         .Where(n => n.MediaType.Id == mediaTypeId.Value);
        }

        if (categoryId.HasValue)
        {
            notes = notes.Include(n => n.Categories)
                         .Where(n => n.Categories.Any(c => c.Id == categoryId.Value));
        }

        return await notes.Select(note => new NoteResponse
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Select(x => x.Name).ToList(),
            MediaType = note.MediaType.TypeName,
        }).ToListAsync();
    }

    public async Task<NoteResponse?> GetByIdAsync(string userId, int id)
    {
        Note? note = await _dbContext.Notes.Include(n => n.MediaType)
                                           .Include(n => n.Categories)
                                           .SingleOrDefaultAsync(n => n.UserId == userId && n.Id == id);

        if (note == null)
        {
            return null;
        }

        return new NoteResponse()
        {
            Id = note.Id,
            NoteKey = note.NoteKey,
            NoteValue = note.NoteValue,
            Categories = note.Categories.Select(x => x.Name).ToList(),
            MediaType = note.MediaType.TypeName,
        };
    }

    // TODO: also figure out how to bypass looking up tables before adding new note (like explicit properties for foreign keys)
    // TODO: Use Note Domain object instead of controller DTO? in That case Entity becomes the domain object that should not be modeled for the EF
    public async Task<NoteResponse> AddAsync(string userId, NotePostRequest request)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }

        var addedNote = await _dbContext.Notes.AddAsync(new()
        {
            UserId = userId,
            NoteKey = request.NoteKey,
            NoteValue = request.NoteValue,
            Categories = await _dbContext.Categories.Where(c => request.CategoryIds.Contains(c.Id)).ToListAsync(),
            MediaType = await _dbContext.MediaTypes.FindAsync(request.MediaTypeId),
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
    public async Task<bool> UpdateAsync(string userId, int id, NotePutRequest request)
    {
        var note = await _dbContext.Notes.Include(n => n.MediaType)
                                         .Include(n => n.Categories)
                                         .FirstOrDefaultAsync(n => n.UserId == userId && n.Id == id);

        if (note == null)
        {
            return false;
        }

        note.NoteValue = request.NoteValue;

        if (note.MediaType.Id != request.MediaTypeId)
        {
            note.MediaType = await _dbContext.MediaTypes.FindAsync(request.MediaTypeId);
        }

        await UpdateCategoriesAsync(note, request);

        // TODO: add check if no changes made then don't save 

        int updated = await _dbContext.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> DeleteAsync(string userId, int id)
    {
        // TODO: split usercheck and the deleting id check, do asnotracking precheck
        var removed = await _dbContext.Notes.Where(n => n.Id == id && n.UserId == userId)
                                         .ExecuteDeleteAsync();
        return removed > 0;
    }

    private async Task UpdateCategoriesAsync(Note note, NotePutRequest request)
    {
        var currentIds = note.Categories.Select(c => c.Id).ToList();
        var categoriesToRemove = note.Categories.Where(c => !request.CategoryIds.Contains(c.Id)).ToList();
        var addedCategories = await _dbContext.Categories.Where(c => request.CategoryIds.Contains(c.Id) && !currentIds.Contains(c.Id))
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
}
