namespace NotesService.Tests.Controllers;

using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesService.API;
using NotesService.API.Controllers;
using NotesService.API.DTO;
using NotesService.Tests.Fixtures;

public class NotesControllerTests
{
    private readonly ILogger<NotesController> _logger;
    private readonly INotesRepository _repository;
    private readonly IValidator<NotePostRequest> _postValidator;

    //System under test
    private readonly NotesController _sut;

    public NotesControllerTests()
    {
        _logger = A.Fake<ILogger<NotesController>>();
        _repository = A.Fake<INotesRepository>();
        _postValidator = A.Fake<IValidator<NotePostRequest>>();
        _sut = new NotesController(_logger, _repository, _postValidator);
    }


    [Fact]
    public async void Get_OnSuccess_ReturnsStatusCode200AndNoteList()
    {
        // Arrange
        A.CallTo(() => _repository.GetAsync(A<string?>._, A<string?>._))
         .Returns(NoteResponseFixtures.GetTestNoteResponses());


        // Act
        IActionResult result = await _sut.GetAsync(null, null);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<NoteResponse>>(((OkObjectResult)result).Value);
        Assert.Equal(200, ((OkObjectResult)result).StatusCode);
    }

    [Fact]
    public async void Get_OnNoData_ReturnsNoContent204()
    {
        // Arrange
        A.CallTo(() => _repository.GetAsync(A<string?>._, A<string?>._))
         .Returns([]);


        // Act
        IActionResult result = await _sut.GetAsync(null, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, ((NoContentResult)result).StatusCode);
    }

    [Fact]
    public async void Get_OnSuccess_InvokesRepositoryOnce()
    {
        // Arrange
        // Act
        IActionResult result = await _sut.GetAsync(null, null);

        // Assert
        A.CallTo(() => _repository.GetAsync(A<string?>._, A<string?>._))
         .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void GetById_OnNotFound_ReturnsStatusCode404()
    {
        // Arrange
        int missingId = 10;

        A.CallTo(() => _repository.GetByIdAsync(missingId))
         .Returns((NoteResponse)null);

        // Act
        IActionResult result = await _sut.GetAsync(missingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, ((NotFoundResult)result).StatusCode);
    }

    [Fact]
    public async void GetById_OnSuccess_ReturnsStatusCode200AndReturnsNoteResponse()
    {
        // Arrange
        int requestedId = 10;
        A.CallTo(() => _repository.GetByIdAsync(requestedId))
         .Returns(NoteResponseFixtures.GetTestNoteResponse());

        // Act
        IActionResult result = await _sut.GetAsync(requestedId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<NoteResponse>(((OkObjectResult)result).Value);
        Assert.Equal(200, ((OkObjectResult)result).StatusCode);
    }

    // TODO: resolve properly test of location setting
    [Fact]
    public async void Post_OnSuccess_ReturnsStatusCode201AndReturnsNoteResponseAndLocationAddress()
    {
        // Arrange
        NotePostRequest note = new()
        {
            NoteKey = "palabra",
            NoteValue = "word",
            Categories = ["word", "spanish"],
            MediaType = "text"
        };

        var validationFake = A.Fake<ValidationResult>();

        A.CallTo(() => _postValidator.Validate(note)).Returns(validationFake);
        A.CallTo(() => validationFake.IsValid).Returns(true);

        A.CallTo(() => _repository.AddAsync(note))
         .Returns(new NoteResponse
         {
             Id = 1,
             NoteKey = note.NoteKey,
             NoteValue = note.NoteValue,
             Categories = note.Categories,
             MediaType = note.MediaType,
         });

        // Act
        IActionResult result = await _sut.PostAsync(note);

        // Assert
        Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, ((CreatedResult)result).StatusCode);
        Assert.IsType<NoteResponse>(((CreatedResult)result).Value);
    }

    [Fact]
    public async void Post_OnInvalidRequest_ReturnsStatusCode400()
    {
        // Arrange
        NotePostRequest note = new();
        var validationFake = A.Fake<ValidationResult>();

        A.CallTo(() => _postValidator.Validate(note)).Returns(validationFake);
        A.CallTo(() => validationFake.IsValid).Returns(false);

        // Act
        IActionResult result = await _sut.PostAsync(note);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
    }

    // TODO: Post Checks if fields valid and returns appropriate status
    // TODO: Updated On Success returns appropriate Code/ On Fail returns approrpriate code
    // TODO: Delete same as Update
}