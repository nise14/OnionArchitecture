using FluentValidation;

namespace Application.Features.Clients.Commands.UpdateClientCommand;

public class UpdateClienteCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClienteCommandValidator()
    {
        RuleFor(p => p.Id)
             .NotEmpty().WithMessage("{PropertyName} can't be empty.");

        RuleFor(p => p.Name)
               .NotEmpty().WithMessage("{PropertyName} can't be empty.")
               .MaximumLength(80).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.LastName)
           .NotEmpty().WithMessage("{PropertyName} can't be empty.")
           .MaximumLength(80).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.DateOfBirth)
           .NotEmpty().WithMessage("DateOfBirth can't be empty");

        RuleFor(p => p.Phone)
           .NotEmpty().WithMessage("{PropertyName} can't be empty.")
           .Matches(@"^\d{4}-\d{4}$").WithMessage("{PropertyName} must has the format 0000-0000")
           .MaximumLength(9).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.Email)
           .NotEmpty().WithMessage("{PropertyName} can't be empty.")
           .EmailAddress().WithMessage("{PropertName} must be a valid email")
           .MaximumLength(100).WithMessage("{PropertyName} can't be more of {MaxLength}");

        RuleFor(p => p.Address)
           .NotEmpty().WithMessage("{PropertyName} can't be empty.")
           .MaximumLength(120).WithMessage("{PropertyName} can't be more of {MaxLength}");
    }
}