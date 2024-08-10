namespace NotesService.API.Abstractions;

using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

public interface INotesRepository
{
    Task<IList<NoteResponse>> GetAsync(string userId, int? mediaTypeId, int? categoryId);

    Task<NoteResponse?> GetByIdAsync(string userId, int id);

    Task<NoteResponse> AddAsync(string userId, NotePostRequest request);

    Task<bool> UpdateAsync(string userId, int id, NotePutRequest request);

    Task<bool> DeleteAsync(string userId, int id);
}
