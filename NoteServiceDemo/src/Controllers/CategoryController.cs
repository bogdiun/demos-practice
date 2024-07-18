﻿namespace NotesService.API.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NotesService.API.Common;
using NotesService.API.Common.DTO.Request;
using NotesService.API.Common.DTO.Response;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryRepository _repository;

    public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryRepository repository)
    {
        _logger = logger;
        _repository = repository;
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
