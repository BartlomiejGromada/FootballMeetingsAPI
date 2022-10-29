using AutoMapper;
using Contracts.Models.Comments;
using Domain.Entities;

namespace Contracts.MappingProfiles;

internal sealed class CommentsMappingProfile : Profile
{
    public CommentsMappingProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dto => dto.UserId, cb => cb.MapFrom(c => c.UserId))
            .ForMember(dto => dto.UserNickName, cb => cb.MapFrom(c => c.User.NickName))
            .ForMember(dto => dto.UserRoleName, cb => cb.MapFrom(c => c.User.Role.Name));

        CreateMap<AddCommentDto, Comment>();

        CreateMap<UpdateCommentDto, Comment>();
    }
}
