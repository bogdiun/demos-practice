namespace NotesService.API.Abstractions;

using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

public interface IMediaTypeRepository
{
    Task<IList<MediaTypeResponse>> GetAllAsync();

    Task<MediaTypeResponse> AddAsync(MediaTypeRequest request);

    Task<bool> UpdateAsync(int id, MediaTypeRequest request);

    Task<bool> DeleteAsync(int id);
}
