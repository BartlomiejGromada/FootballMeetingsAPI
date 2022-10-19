using Contracts;
using Domain.Entities;

namespace Services.MappingProfiles;

internal sealed class UserMappingProfile : MappingProfile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, PlayerDto>();
    }
}
