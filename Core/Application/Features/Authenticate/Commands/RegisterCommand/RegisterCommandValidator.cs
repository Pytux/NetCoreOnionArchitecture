using FluentValidation;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("{PropertyName} can't be empty")
            .MaximumLength(80).WithMessage("{PropertyName} max length {MaxLength}");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} can't be empty")
            .EmailAddress().WithMessage("{PropertyName} is not valid email address")
            .MaximumLength(100).WithMessage("{PropertyName} max length {MaxLength}");

        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("{PropertyName} can't be empty")
            .MaximumLength(10).WithMessage("{PropertyName} max length {MaxLength}");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("{PropertyName}can't be empty")
            .MaximumLength(15).WithMessage("{PropertyName} max length {MaxLength}");
    }
}