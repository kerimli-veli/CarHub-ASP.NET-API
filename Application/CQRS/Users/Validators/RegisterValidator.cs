using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.CQRS.Users.Validators;

public class RegisterValidator : AbstractValidator<Register.RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.Surname)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(u => u.Phone)
            .MaximumLength(20)
            .Matches(@"^\+?[0-9\s-]*$");

        //RuleFor(u => u.UserImagePath)
        //    .MaximumLength(500);

    }
}
