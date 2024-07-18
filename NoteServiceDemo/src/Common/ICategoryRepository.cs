namespace NotesService.API.Common;

using NotesService.API.Common.DTO.Request;
using NotesService.API.Common.DTO.Response;

public interface ICategoryRepository
{
    Task<IList<CategoryResponse>> GetAllAsync();

    Task<CategoryResponse> AddAsync(CategoryRequest request);

    Task<bool> UpdateAsync(int id, CategoryRequest request);

    Task<bool> DeleteAsync(int id);
}
