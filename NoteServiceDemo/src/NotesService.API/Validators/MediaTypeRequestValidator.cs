namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Request;

public class MediaTypeRequestValidator : AbstractValidator<MediaTypeRequest>
{
    public MediaTypeRequestValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 50);
    }
}
