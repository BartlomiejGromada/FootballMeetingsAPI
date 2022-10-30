using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class AccountsRepository : IAccountsRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public AccountsRepository(FootballMeetingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> RegisterUser(User user)
    {
        await _dbContext.Users
            .AddAsync(user);

        return user.Id;
    }

    public async Task RemoveUserById(int userId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);

        user.IsActive = false;
    }

    public async Task RestoreUserById(int userId)
    {
        var user = await _dbContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(user => user.Id == userId && user.IsActive == false);

        user.IsActive = true;
    }
}
