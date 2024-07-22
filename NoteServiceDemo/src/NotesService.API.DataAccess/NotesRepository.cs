﻿namespace NotesService.API.DataAccess;

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

    public async Task<IList<NoteResponse>> GetAsync(int? mediaTypeId, int? categoryId)
    {
        // TODO: filtering
        // TODO implement pagination, cancellation
        // TODO mappers probably not necessary
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

    // TODO: also figure out how to bypass looking up tables before adding new note (like explicit properties for foreign keys)
    public async Task<NoteResponse> AddAsync(NotePostRequest request)
    {
        // TODO: rename to plural (by creating without props it made singular
        var addedNote = await _dbContext.Notes.AddAsync(new()
        {
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
    // TODO: only change actually where needed categories/mediatype/value - work out how it is best to do
    public async Task<bool> UpdateAsync(int id, NotePutRequest request)
    {
        var note = await _dbContext.Notes.Include(n => n.MediaType)
                                         .Include(n => n.Categories)
                                         .FirstOrDefaultAsync(n => n.Id == id);

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

    public async Task<bool> DeleteAsync(int id)
    {
        var removed = await _dbContext.Notes.Where(n => n.Id == id)
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
