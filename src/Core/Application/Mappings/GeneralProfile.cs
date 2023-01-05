using Application.Dtos;
using Application.Features.Clients.Commands.CreateClientCommand;
using AutoMapper;
using Domain.Client;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region  Dtos
        CreateMap<Client, ClientDto>();
        #endregion

        #region Commands
        CreateMap<CreateClientCommand, Client>();
        #endregion
    }
}