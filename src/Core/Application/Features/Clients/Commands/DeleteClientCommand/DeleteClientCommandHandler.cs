using Application.Interfaces;
using Application.Wrappers;
using Domain.Client;
using MediatR;

namespace Application.Features.Clients.Commands.DeleteClientCommand;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Response<int>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;

    public DeleteClientCommandHandler(IRepositoryAsync<Client> repositoryAsync)
    {
        _repositoryAsync = repositoryAsync;
    }

    public async Task<Response<int>> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _repositoryAsync.GetByIdAsync(request.Id);

        if (client is null)
        {
            throw new KeyNotFoundException($"Record not found with id {request.Id}");
        }
        else
        {
            await _repositoryAsync.DeleteAsync(client);
            return new Response<int>(client.Id);
        }
    }
}