using AutoMapper;
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
        }
    }
}