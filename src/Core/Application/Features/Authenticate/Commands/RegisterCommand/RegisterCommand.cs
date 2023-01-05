using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommand : IRequest<Response<string>>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Origin { get; set; }
}