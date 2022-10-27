﻿using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public UsersRepository(FootballMeetingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AnyAsync(user => user.Id == userId && user.IsActive, cancellationToken);
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower() && user.IsActive, cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(int userId, bool isActive = true, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId && user.IsActive == isActive, cancellationToken);
    }
}
