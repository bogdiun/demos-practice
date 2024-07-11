namespace NotesService.API.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> _logger;

    public NotesController(ILogger<NotesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets all notes for the user
    /// </summary>
    /// <returns>id's and descriptions of all notes</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    // TODO: Add [ProducesResponseType(200, typeof(ExampleTypeObject))]
    public async Task<IActionResult> GetAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets note with specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Note details</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        // TODO: change id to from query?
        throw new NotImplementedException();
    }

    /// <summary>
    /// Posts a note in the body for storage
    /// </summary>
    /// <param name="body">Note body</param>
    /// <returns>Note id</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] string body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the specified note (by id)
    /// </summary>
    /// <param name="id">id of a note to update</param>
    /// <param name="body">new note details</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] string body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes the specified note
    /// </summary>
    /// <param name="id">id of the note to delete</param>
    /// <returns>details of a deleted note?</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    //[HttpPatch]
}
