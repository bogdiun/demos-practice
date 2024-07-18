namespace NotesService.API.Abstractions;

using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

// API described Interface, could return the Result in the way Controller wants it?
// Or if we keep DataAccess in here and don't care about changing it then we might remove it and do directly
// TODO: separate into separate project NoteService.DataAccess, and then this one would need to go somewhere to NoteService.Abstractions/Commons?
public interface INotesRepository
{
    Task<IList<NoteResponse>> GetAsync(string? mediaType, string? category);

    Task<NoteResponse> GetByIdAsync(int id);

    Task<NoteResponse> AddAsync(NotePostRequest request);

    Task<bool> UpdateAsync(int id, NotePutRequest request);

    Task<bool> DeleteAsync(int id);
}
