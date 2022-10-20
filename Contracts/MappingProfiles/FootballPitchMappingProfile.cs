using Contracts.Models.FootballPitch;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class FootballPitchMappingProfile : MappingProfile
{
    public FootballPitchMappingProfile()
    {
        CreateMap<FootballPitch, FootballPitchDto>();
        CreateMap<AddFootballPitchDto, FootballPitch>();
        CreateMap<UpdateFootballPitchDto, FootballPitch>();
    }
}
