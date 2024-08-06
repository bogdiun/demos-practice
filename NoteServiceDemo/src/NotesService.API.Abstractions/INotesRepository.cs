namespace NotesService.API.Abstractions;

using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

public interface INotesRepository
{
    Task<IList<NoteResponse>> GetAsync(int? mediaTypeId, int? categoryId);

    Task<NoteResponse> GetByIdAsync(int id);

    Task<NoteResponse> AddAsync(NotePostRequest request);

    Task<bool> UpdateAsync(int id, NotePutRequest request);

    Task<bool> DeleteAsync(int id);
}
