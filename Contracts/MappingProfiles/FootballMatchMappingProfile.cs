using Contracts.Models.FootballMatch;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class FootballMatchMappingProfile : MappingProfile
{
	public FootballMatchMappingProfile()
	{
		CreateMap<FootballMatch, FootballMatchDto>();
		CreateMap<AddFootballMatchDto, FootballMatch>();
		CreateMap<UpdateFootballMatchDto, FootballMatch>();
	}
}
