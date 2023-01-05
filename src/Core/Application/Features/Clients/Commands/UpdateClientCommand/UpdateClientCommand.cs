using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Commands.UpdateClientCommand;

public class UpdateClientCommand : IRequest<Response<int>>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}