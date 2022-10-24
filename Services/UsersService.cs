using AutoMapper;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public class UsersService : IUsersService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    public UsersService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task RemoveUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UsersRepository.GetUserByIdAsync(userId, cancellationToken);

        if(user is null)
        {
            throw new NotFoundException($"User with id {userId} cannot be found");
        }

        await _repositoryManager.UsersRepository.RemoveUserByIdAsync(userId, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
