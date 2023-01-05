using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Commands.DeleteClientCommand;

public class DeleteClientCommand : IRequest<Response<int>>
{
    public int Id { get; set; }
}