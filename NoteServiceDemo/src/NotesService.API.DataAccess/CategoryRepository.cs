namespace NotesService.API.DataAccess;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dbContext;

    public CategoryRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<CategoryResponse>> GetAllAsync()
    {
        return await _dbContext.Categories.Select(m => new CategoryResponse
        {
            Id = m.Id,
            Name = m.Name,
        }).ToListAsync();
    }

    public async Task<CategoryResponse> AddAsync(CategoryRequest request)
    {
        var added = await _dbContext.Categories.AddAsync(new()
        {
            Name = request.Name,
        });

        var created = await _dbContext.SaveChangesAsync();

        if (created <= 0)
        {
            return null;
        }

        return new()
        {
            Id = added.Entity.Id,
            Name = added.Entity.Name,
        };
    }

    public async Task<bool> UpdateAsync(int id, CategoryRequest request)
    {
        int updated = await _dbContext.Categories.Where(n => n.Id == id)
                                                 .ExecuteUpdateAsync(c =>
                                                    c.SetProperty(p => p.Name, request.Name));
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var removed = await _dbContext.Categories.Where(n => n.Id == id)
                                                 .ExecuteDeleteAsync();
        return removed > 0;
    }
}
