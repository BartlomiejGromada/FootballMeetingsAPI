using Domain.Repositories;

namespace Persistence.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly FootballMeetingsDbContext _dbContext;

    public UnitOfWork(FootballMeetingsDbContext dbContext) => _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);        
}
