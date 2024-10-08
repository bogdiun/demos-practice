﻿namespace NotesService.API.Validators;

using FluentValidation;
using NotesService.API.Abstractions.DTO.Auth;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Must be an email");
    }
}
