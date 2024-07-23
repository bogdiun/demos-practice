namespace NotesService.API.Controllers;

using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryRepository _repository;
    private readonly IValidator<CategoryRequest> _requestValidator;

    public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryRepository repository,
            IValidator<CategoryRequest> requestValidator)
    {
        _logger = logger;
        _repository = repository;
        _requestValidator = requestValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IList<CategoryResponse> results = await _repository.GetAllAsync();

        if (results?.Any() != true)
        {
            return NoContent();
        }

        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CategoryRequest postRequest)
    {
        var validationResult = _requestValidator.Validate(postRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        CategoryResponse response = await _repository.AddAsync(postRequest);

        if (response == null)
        {
            return BadRequest();
        }

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] CategoryRequest putRequest)
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
