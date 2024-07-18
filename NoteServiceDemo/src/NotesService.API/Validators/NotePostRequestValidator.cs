namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Request;

public class NotePostRequestValidator : AbstractValidator<NotePostRequest>
{
    public NotePostRequestValidator()
    {

        RuleFor(x => x.NoteKey)
            .NotEmpty()
            .Length(1, 50);

        RuleFor(x => x.MediaTypeId)
            .NotEmpty();

        RuleFor(x => x.NoteValue)
            .NotEmpty();
    }
}
