using System.Text;
using System.Text.Json;
using Application.Dtos;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Client;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PagedResponse<List<ClientDto>>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;

    public GetAllClientsQueryHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
    {
        _repositoryAsync = repositoryAsync;
        _distributedCache = distributedCache;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<ClientDto>>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ClientList_{request.PageSize}_{request.PageNumber}_{request.Name}_{request.LastName}";
        string serializedListClient;
        var listClients = new List<Client>();
        var redisListClients = await _distributedCache.GetAsync(cacheKey);

        if (redisListClients is not null)
        {
            serializedListClient = Encoding.UTF8.GetString(redisListClients);
            listClients = JsonSerializer.Deserialize<List<Client>>(serializedListClient);
        }
        else
        {
            listClients = await _repositoryAsync.ListAsync(new PagedClientsSpecification(request.PageSize, request.PageNumber, request.Name, request.LastName));
            serializedListClient = JsonSerializer.Serialize(listClients);
            redisListClients = Encoding.UTF8.GetBytes(serializedListClient);

            var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            await _distributedCache.SetAsync(cacheKey, redisListClients, options);
        }

        var clientsDto = _mapper.Map<List<ClientDto>>(listClients);

        return new PagedResponse<List<ClientDto>>(clientsDto, request.PageNumber, request.PageSize);
    }
}