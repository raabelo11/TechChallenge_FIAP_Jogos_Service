using AutoMapper;
using Jogos.Service.Application.Dtos;
using Jogos.Service.Domain.Models;

namespace Jogos.Service.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Jogo, JogoDto>().ReverseMap().ForMember("Id", opt => opt.Ignore());
            CreateMap<JogoRequest, Jogo>().ReverseMap();
            CreateMap<PedidoEventDto, PedidoEvent>().ForMember("Id", opt => opt.Ignore());
        }
    }
}
