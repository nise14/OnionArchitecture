using Application.Dtos;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<PagedResponse<List<ClientDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
}