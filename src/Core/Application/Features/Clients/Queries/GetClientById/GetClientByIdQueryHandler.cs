using Application.Dtos;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Client;
using MediatR;

namespace Application.Features.Clients.Queries.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Response<ClientDto>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;
    private readonly IMapper _mapper;

    public GetClientByIdQueryHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper)
    {
        _repositoryAsync = repositoryAsync;
        _mapper = mapper;
    }

    public async Task<Response<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _repositoryAsync.GetByIdAsync(request.Id);

        if (client is not null)
        {
            var clientMappedtoDto = _mapper.Map<ClientDto>(client);
            return new Response<ClientDto>(clientMappedtoDto);
        }

        throw new KeyNotFoundException($"Record not found with id {request.Id}");
    }
}