using Contracts.FootballMatch;
using Domain.Entities;

namespace Services.MappingProfiles;

internal sealed class FootballMatchMappingProfile : MappingProfile
{
	public FootballMatchMappingProfile()
	{
		CreateMap<FootballMatch, FootballMatchDto>();
		CreateMap<AddFootballMatchDto, FootballMatch>();
		CreateMap<UpdateFootballMatchDto, FootballMatch>();
	}
}
