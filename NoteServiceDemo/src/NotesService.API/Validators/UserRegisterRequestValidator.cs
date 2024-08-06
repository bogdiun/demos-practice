namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Auth;

public class UserRegisterRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Must be an email");
    }
}