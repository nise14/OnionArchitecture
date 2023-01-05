using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Commands.CreateClientCommand;

public class CreateClientCommand : IRequest<Response<int>>
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}