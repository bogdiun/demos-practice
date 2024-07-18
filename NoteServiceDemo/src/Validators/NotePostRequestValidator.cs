namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Common.DTO.Request;

public class NotePostRequestValidator : AbstractValidator<NotePostRequest>
{
    public NotePostRequestValidator()
    {
        RuleFor(x => x.MediaType)
            .NotEmpty()
            .Matches("[a-zA-Z_]+");

        RuleFor(x => x.NoteValue)
            .NotEmpty();

        RuleFor(x => x.NoteKey)
            .NotEmpty();
    }
}
