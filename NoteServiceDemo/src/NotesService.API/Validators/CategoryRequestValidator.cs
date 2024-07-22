namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Request;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 50).WithMessage("Must be max 50 characters");
    }
}
