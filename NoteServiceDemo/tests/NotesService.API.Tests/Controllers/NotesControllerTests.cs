namespace NotesService.Tests.Controllers;

using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesService.API.Abstractions;
using NotesService.API.Abstractions.DTO.Request;
using NotesService.API.Abstractions.DTO.Response;
using NotesService.API.Controllers;
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
    public async Task Get_OnSuccess_ReturnsStatusCode200AndNoteList()
    {
        // Arrange
        A.CallTo(() => _repository.GetAsync(A<string>._, A<int?>._, A<int?>._))
         .Returns(NoteResponseFixtures.GetTestNoteResponses());

        // Act
        IActionResult result = await _sut.GetAsync(null, null);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<NoteResponse>>(((OkObjectResult)result).Value);
        Assert.Equal(200, ((OkObjectResult)result).StatusCode);
    }

    [Fact]
    public async Task Get_OnNoData_ReturnsNoContent204()
    {
        // Arrange
        A.CallTo(() => _repository.GetAsync(A<string>._, A<int?>._, A<int?>._))
         .Returns([]);


        // Act
        IActionResult result = await _sut.GetAsync(null, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, ((NoContentResult)result).StatusCode);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesRepositoryOnce()
    {
        // Arrange
        // Act
        _ = await _sut.GetAsync(null, null);

        // Assert
        A.CallTo(() => _repository.GetAsync(A<string>._, A<int?>._, A<int?>._))
         .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetById_OnNotFound_ReturnsStatusCode404()
    {
        // Arrange
        const int missingId = 10;

        A.CallTo(() => _repository.GetByIdAsync(A<string>._, missingId))
         .Returns((NoteResponse)null);

        // Act
        IActionResult result = await _sut.GetAsync(missingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, ((NotFoundResult)result).StatusCode);
    }

    [Fact]
    public async Task GetById_OnSuccess_ReturnsStatusCode200AndReturnsNoteResponse()
    {
        // Arrange
        const int requestedId = 10;
        A.CallTo(() => _repository.GetByIdAsync(A<string>._, requestedId))
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
    public async Task Post_OnSuccess_ReturnsStatusCode201AndReturnsNoteResponseAndLocationAddress()
    {
        // Arrange
        NotePostRequest note = new()
        {
            NoteKey = "palabra",
            NoteValue = "word",
            CategoryIds = [1, 3],
            MediaTypeId = 1
        };

        var validationFake = A.Fake<ValidationResult>();

        A.CallTo(() => _postValidator.Validate(note)).Returns(validationFake);
        A.CallTo(() => validationFake.IsValid).Returns(true);

        A.CallTo(() => _repository.AddAsync(A<string>._, note))
         .Returns(new NoteResponse
         {
             Id = 1,
             NoteKey = "key",
             NoteValue = "value",
             Categories = ["m", "n"],
             MediaType = "text",
         });

        // Act
        IActionResult result = await _sut.PostAsync(note);

        // Assert
        Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, ((CreatedResult)result).StatusCode);
        Assert.IsType<NoteResponse>(((CreatedResult)result).Value);
    }

    [Fact]
    public async Task Post_OnInvalidRequest_ReturnsStatusCode400()
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