using AutoMapper;
using Contracts.Models.FootballMatch;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class FootballMatchMappingProfile : Profile
{
	public FootballMatchMappingProfile()
	{
		CreateMap<FootballMatch, FootballMatchDto>();
		CreateMap<AddFootballMatchDto, FootballMatch>();
		CreateMap<UpdateFootballMatchDto, FootballMatch>();
	}
}
