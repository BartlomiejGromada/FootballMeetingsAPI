using AutoMapper;
using Contracts.Models.Account;
using Contracts.Models.User;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, PlayerDto>();
        CreateMap<UserDto, User>();
        CreateMap<RegisterUserDto, User>();
    }
}
