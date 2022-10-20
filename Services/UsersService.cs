using AutoMapper;
using Contracts.Models.User;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;

namespace Services;

public class UsersService : IUsersService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher; 
    public UsersService(IRepositoryManager repositoryManager, IMapper mapper, IPasswordHasher<User> passwordHasher)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        await _repositoryManager.UsersRepository.RegisterUserAsync(user, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
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
