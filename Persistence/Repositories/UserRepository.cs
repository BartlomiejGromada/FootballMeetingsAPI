using Domain.Entities;
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
            .AnyAsync(user => user.Id == userId, cancellationToken);
    }

    public async Task RegisterUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .AddAsync(user, cancellationToken);
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);

        return user;
    }

    public async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public async Task RemoveUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        user.IsActive = false;
    }
}
