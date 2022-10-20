using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class FootballMatchesRepository : IFootballMatchesRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public FootballMatchesRepository(FootballMeetingsDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<FootballMatch>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .Include(fm => fm.FootballPitch)
            .Include(fm => fm.Creator)
            .Where(fm => fm.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FootballMatch>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .Include(fm => fm.FootballPitch)
            .Include(fm => fm.Creator)
            .Where(fm => fm.CreatorId == creatorId && fm.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<FootballMatch> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .Include(fm => fm.FootballPitch)
            .Include(fm => fm.Creator)
            .Where(fm => fm.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId, cancellationToken);
    }
    public async Task AddAsync(FootballMatch footballMatch, CancellationToken cancellationToken = default)
    {
        await _dbContext.FootballMatches.AddAsync(footballMatch, cancellationToken);
    }

    public async Task RemoveByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _dbContext.FootballMatches
            .FirstOrDefaultAsync(cancellationToken);

        footballMatch.IsActive = false;
    }

    public async Task UpdateAsync(int footballMatchId, FootballMatch footballMatch)
    {
        var searchedFootballMatch = await _dbContext.FootballMatches
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId);

        searchedFootballMatch.Name = footballMatch.Name;
        searchedFootballMatch.MaxNumberOfPlayers = footballMatch.MaxNumberOfPlayers;
        searchedFootballMatch.Date = footballMatch.Date;
        searchedFootballMatch.FootballPitchId = footballMatch.FootballPitchId;
    }
}
