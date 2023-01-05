using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Client;
using MediatR;

namespace Application.Features.Clients.Commands.UpdateClientCommand;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Response<int>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper)
    {
        _repositoryAsync = repositoryAsync;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _repositoryAsync.GetByIdAsync(request.Id);

        if (client is null)
        {
            throw new KeyNotFoundException($"Record not found with id {request.Id}");
        }
        else
        {
            client.Name = request.Name;
            client.LastName = request.LastName;
            client.DateOfBirth = request.DateOfBirth;
            client.Phone = request.Phone;
            client.Email = request.Email;
            client.Address = request.Address;

            await _repositoryAsync.UpdateAsync(client);

            return new Response<int>(client.Id);
        }
    }
}