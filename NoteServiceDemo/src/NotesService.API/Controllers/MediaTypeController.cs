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
public class MediaTypeController : ControllerBase
{
    private readonly ILogger<MediaTypeController> _logger;
    private readonly IMediaTypeRepository _repository;
    private readonly IValidator<MediaTypeRequest> _requestValidator;

    public MediaTypeController(
            ILogger<MediaTypeController> logger,
            IMediaTypeRepository repository,
            IValidator<MediaTypeRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _requestValidator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<MediaTypeResponse> results = await _repository.GetAllAsync();

        if (results?.Any() != true)
        {
            return NoContent();
        }

        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] MediaTypeRequest postRequest)
    {
        var validationResult = _requestValidator.Validate(postRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        MediaTypeResponse response = await _repository.AddAsync(postRequest);

        if (response == null)
        {
            return BadRequest();
        }

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] MediaTypeRequest putRequest)
    {
        var validationResult = _requestValidator.Validate(putRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        bool isUpdated = await _repository.UpdateAsync(id, putRequest);

        if (isUpdated)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        bool isUpadated = await _repository.DeleteAsync(id);

        if (isUpadated)
        {
            return NoContent();
        }

        return NotFound();
    }
}