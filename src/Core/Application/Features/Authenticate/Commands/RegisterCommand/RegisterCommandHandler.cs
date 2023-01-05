using Application.Dtos.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<string>>
{
    private readonly IAccountService _accountService;

    public RegisterCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.RegisterAsync(new RegisterRequest
        {
            Email = request.Email,
            ConfirmPassword = request.ConfirmPassword,
            LastName = request.LastName,
            Name = request.Name,
            Password = request.Password,
            UserName = request.UserName
        }, request.Origin);
    }
}