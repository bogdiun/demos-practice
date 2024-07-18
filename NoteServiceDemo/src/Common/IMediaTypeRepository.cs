namespace NotesService.API.Common;

using NotesService.API.Common.DTO.Request;
using NotesService.API.Common.DTO.Response;

public interface IMediaTypeRepository
{
    Task<IList<MediaTypeResponse>> GetAllAsync();

    Task<MediaTypeResponse> AddAsync(MediaTypeRequest request);

    Task<bool> UpdateAsync(int id, MediaTypeRequest request);

    Task<bool> DeleteAsync(int id);
}
