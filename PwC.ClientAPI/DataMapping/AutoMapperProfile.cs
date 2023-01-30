using AutoMapper;
using PwC.ClientAPI.Domain.Models;

namespace PwC.ClientAPI.DataMapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClientRequestObject, Client>();
            CreateMap<Client, ClientResponseObject>();
        }
    }
}
