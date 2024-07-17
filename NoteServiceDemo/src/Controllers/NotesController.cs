namespace NotesService.API.Controllers;

using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotesService.API;
using NotesService.API.DTO;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> _logger;
    private readonly INotesRepository _notesRepository;
    private readonly IValidator<NotePostRequest> _postRequestValidator;

    public NotesController(ILogger<NotesController> logger,
                           INotesRepository notesRepository,
                           IValidator<NotePostRequest> postRequestValidator)
    {
        _logger = logger;
        _notesRepository = notesRepository;
        _postRequestValidator = postRequestValidator;
    }

    /// <summary>
    /// Gets all notes for the user
    /// </summary>
    /// <returns>id's and descriptions of all notes</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    // TODO: Add [ProducesResponseType(200, typeof(ExampleTypeObject))]
    public async Task<IActionResult> GetAsync([FromQuery] string? mediaType, [FromQuery] string? category)
    {
        // TODO manage users

        IList<NoteResponse> results = await _notesRepository.GetAsync(mediaType, category);

        if (results?.Any() != true)
        {
            return NoContent();
        }

        return Ok(results);
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
        NoteResponse result = await _notesRepository.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Posts a note in the body for storage
    /// </summary>
    /// <param name="postRequest"></param>
    /// <returns>Note id</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] NotePostRequest postRequest)
    {
        var validationResult = _postRequestValidator.Validate(postRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        NoteResponse postedNote = await _notesRepository.AddAsync(postRequest);

        if (postedNote == null)
        {
            return BadRequest();
        }

        // TODO: later adjust it to return full uri, without using HttpContext
        return Created($"notes/{postedNote.Id}", postedNote);
    }

    /// <summary>
    /// Updates the specified note (by id)
    /// </summary>
    /// <param name="id">id of a note to update</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] NotePutRequest putRequest)
    {
        bool isUpdated = await _notesRepository.UpdateAsync(id, putRequest);

        if (isUpdated)
        {
            return NoContent();
        }

        return NotFound();
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
        bool isUpadated = await _notesRepository.DeleteAsync(id);

        if (isUpadated)
        {
            return NoContent();
        }

        return NotFound();
    }
}
