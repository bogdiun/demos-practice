using Microsoft.AspNetCore.Mvc;

namespace NotesService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NoteController : ControllerBase
{
    private readonly ILogger<NoteController> _logger;

    public NoteController(ILogger<NoteController> logger)
    {
        _logger = logger;
    }

}
