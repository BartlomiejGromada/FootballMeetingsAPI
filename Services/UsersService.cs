using AutoMapper;
using Contracts.Models.User;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public sealed class UsersService : IUsersService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UsersService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<UserDto> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UsersRepository
            .GetUserByIdAsync(userId, isActive: true, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {userId} cannot be found");
        }

        return _mapper.Map<UserDto>(user);
    }
}
