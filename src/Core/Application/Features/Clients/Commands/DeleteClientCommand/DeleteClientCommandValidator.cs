using FluentValidation;

namespace Application.Features.Clients.Commands.DeleteClientCommand;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("{PropertyName} can't be empty");
    }
}