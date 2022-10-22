using AutoMapper;
using Contracts.Models.FootballPitch;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class FootballPitchMappingProfile : Profile
{
    public FootballPitchMappingProfile()
    {
        CreateMap<FootballPitch, FootballPitchDto>();
        CreateMap<FootballPitchDto, FootballPitch>();
        CreateMap<AddFootballPitchDto, FootballPitch>();
        CreateMap<UpdateFootballPitchDto, FootballPitch>();
    }
}
