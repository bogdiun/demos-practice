namespace NotesService.API.Abstractions.DTO.Response;

using System.Net;

public record ErrorResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string Message { get; init; }
}
