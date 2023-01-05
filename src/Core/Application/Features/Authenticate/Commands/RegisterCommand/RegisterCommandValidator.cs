using FluentValidation;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(p => p.Name)
        .NotEmpty().WithMessage("{PropertyName} can't be empty")
        .MaximumLength(80).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.LastName)
        .NotEmpty().WithMessage("{PropertyName} can't be empty")
        .MaximumLength(80).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.Email)
       .NotEmpty().WithMessage("{PropertyName} can't be empty")
       .EmailAddress().WithMessage("{PropertName} must be a valid email")
       .MaximumLength(100).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.UserName)
         .NotEmpty().WithMessage("{PropertyName} can't be empty")
         .MaximumLength(10).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.Password)
        .NotEmpty().WithMessage("{PropertyName} can't be empty")
        .MaximumLength(15).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.ConfirmPassword)
        .NotEmpty().WithMessage("{PropertyName} can't be empty")
        .MaximumLength(15).WithMessage("{PropertyName} can't be more of {MaxLength}")
        .Equal(p=>p.Password).WithMessage("{PropertyName} must be equals to Password");
    }
}