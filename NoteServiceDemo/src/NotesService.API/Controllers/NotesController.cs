namespace NotesService.API.Controllers;

using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> _logger;
    private readonly INotesRepository _repository;
    private readonly IValidator<NotePostRequest> _postRequestValidator;

    public NotesController(
            ILogger<NotesController> logger,
            INotesRepository notesRepository,
            IValidator<NotePostRequest> postRequestValidator)
    {
        _logger = logger;
        _repository = notesRepository;
        _postRequestValidator = postRequestValidator;
    }

    // TODO: where does the user Id sit?
    private string? UserId => User?.Claims.Single(c => c.Type == "id").Value;

    /// <summary>
    /// Gets all notes for the current logged in user
    /// </summary>
    /// <returns>id's and descriptions of all notes</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    // TODO: Add [ProducesResponseType(200, typeof(ExampleTypeObject))]
    public async Task<IActionResult> GetAsync([FromQuery] int? mediaTypeId, [FromQuery] int? categoryId)
    {
        //TODO: do I check any claims here?
        IList<NoteResponse> results = await _repository.GetAsync(UserId, mediaTypeId, categoryId);

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
    [HttpGet("{id}", Name = "GetNote")]
    public async Task<IActionResult> GetAsync(int id)
    {
        NoteResponse result = await _repository.GetByIdAsync(UserId, id);

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
        // TODO manage exceptions properly, in case object is created but exception is thrown ie.
        var validationResult = _postRequestValidator.Validate(postRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        NoteResponse created = await _repository.AddAsync(UserId, postRequest);

        if (created == null)
        {
            return BadRequest();
        }

        return CreatedAtRoute("GetNote", new { id = created.Id }, created);
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
        bool isUpdated = await _repository.UpdateAsync(UserId, id, putRequest);

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
        bool isUpadated = await _repository.DeleteAsync(UserId, id);

        if (isUpadated)
        {
            return NoContent();
        }

        return NotFound();
    }
}
