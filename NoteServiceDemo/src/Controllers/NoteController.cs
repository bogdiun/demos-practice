using Microsoft.AspNetCore.Mvc;

namespace NotesService.API.Controllers;

// http://localhost:40381/note
[ApiController]
[Route("notes")]
public class NoteController : ControllerBase
{
    private readonly ILogger<NoteController> _logger;

    public NoteController(ILogger<NoteController> logger)
    {
        _logger = logger;
    }

    // GET: http://localhost:1234/note
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        //
        return Ok();
    }
}
