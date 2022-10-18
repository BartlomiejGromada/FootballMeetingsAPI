using Contracts.FootballPitch;
using Domain.Entities;

namespace Services.MappingProfiles;

internal sealed class FootballPitchMappingProfile : MappingProfile
{
    public FootballPitchMappingProfile()
    {
        CreateMap<FootballPitch, FootballPitchDto>();
        CreateMap<AddFootballPitchDto, FootballPitch>();
        CreateMap<UpdateFootballPitchDto, FootballPitch>();
    }
}
