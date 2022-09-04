using AutoMapper;
using Commands;
using CommandsServ.DataLayerModels;
using CommandsServ.Dtos;

namespace CommandsServ.Profiles
{
    public class CommandsProfiles : Profile
    {
        public CommandsProfiles()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(destination => destination.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<CommandsModel, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.CommandsId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}