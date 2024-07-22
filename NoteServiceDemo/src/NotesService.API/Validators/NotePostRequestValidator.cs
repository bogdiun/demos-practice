namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Request;

public class NotePostRequestValidator : AbstractValidator<NotePostRequest>
{
    public NotePostRequestValidator()
    {
        RuleFor(x => x.NoteKey)
            .NotEmpty().WithMessage("Must not be empty")
            .Length(1, 50).WithMessage("Must be max 50 characters");

        RuleFor(x => x.MediaTypeId)
            .NotEmpty();

        RuleFor(x => x.NoteValue)
            .NotEmpty();
    }
}
