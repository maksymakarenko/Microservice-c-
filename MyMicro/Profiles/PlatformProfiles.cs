using AutoMapper;
using MyMicro.Dtos;
using MyMicro.Models;

namespace MyMicro.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}