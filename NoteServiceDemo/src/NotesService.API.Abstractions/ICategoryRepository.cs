namespace NotesService.API.Abstractions;

using NotesService.API.Abstractions.DTO.Response;
using NotesService.API.Abstractions.DTO.Request;

public interface ICategoryRepository
{
    Task<IList<CategoryResponse>> GetAllAsync();

    Task<CategoryResponse> AddAsync(CategoryRequest request);

    Task<bool> UpdateAsync(int id, CategoryRequest request);

    Task<bool> DeleteAsync(int id);
}
