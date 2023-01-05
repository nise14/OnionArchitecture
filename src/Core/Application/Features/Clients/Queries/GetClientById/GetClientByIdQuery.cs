using Application.Dtos;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Queries.GetClientById;

public class GetClientByIdQuery : IRequest<Response<ClientDto>>
{
    public int Id { get; set; }
}