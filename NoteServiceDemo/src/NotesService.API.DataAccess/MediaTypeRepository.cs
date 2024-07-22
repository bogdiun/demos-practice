namespace NotesService.API.DataAccess;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

internal sealed class MediaTypeRepository : IMediaTypeRepository
{
    private readonly DataContext _dbContext;

    public MediaTypeRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<MediaTypeResponse>> GetAllAsync()
    {
        return await _dbContext.MediaTypes.Select(m => new MediaTypeResponse
        {
            Id = m.Id,
            Name = m.TypeName,
        }).ToListAsync();
    }

    public async Task<MediaTypeResponse> AddAsync(MediaTypeRequest request)
    {
        var added = await _dbContext.MediaTypes.AddAsync(new()
        {
            TypeName = request.Name,
        });

        var created = await _dbContext.SaveChangesAsync();

        if (created <= 0)
        {
            return null;
        }

        return new MediaTypeResponse()
        {
            Id = added.Entity.Id,
            Name = added.Entity.TypeName,
        };
    }

    public async Task<bool> UpdateAsync(int id, MediaTypeRequest request)
    {
        int updated = await _dbContext.MediaTypes.Where(n => n.Id == id)
                                                 .ExecuteUpdateAsync(c =>
                                                    c.SetProperty(p => p.TypeName, request.Name));
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var removed = await _dbContext.MediaTypes.Where(n => n.Id == id)
                                                 .ExecuteDeleteAsync();
        return removed > 0;
    }
}
