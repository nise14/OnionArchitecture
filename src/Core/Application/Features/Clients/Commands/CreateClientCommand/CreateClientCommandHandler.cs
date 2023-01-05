using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Client;
using FluentValidation;
using MediatR;

namespace Application.Features.Clients.Commands.CreateClientCommand;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Response<int>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;
    private readonly IMapper _mapper;

    public CreateClientCommandHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper)
    {
        _repositoryAsync = repositoryAsync;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var newRecord = _mapper.Map<Client>(request);
        var data = await _repositoryAsync.AddAsync(newRecord);

        return new Response<int>(data.Id);
    }
}