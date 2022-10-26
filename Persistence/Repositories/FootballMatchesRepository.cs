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
    public async Task Add(FootballMatch footballMatch)
    {
        foreach (var player in footballMatch.Players)
        {
            _dbContext.Entry(player).State = EntityState.Unchanged;
        }
        await _dbContext.FootballMatches.AddAsync(footballMatch);
    }

    public async Task RemoveById(int footballMatchId)
    {
        var footballMatch = await _dbContext.FootballMatches
            .FirstOrDefaultAsync();

        footballMatch.IsActive = false;
    }

    public async Task Update(int footballMatchId, FootballMatch footballMatch)
    {
        var searchedFootballMatch = await _dbContext.FootballMatches
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId);

        searchedFootballMatch.Name = footballMatch.Name;
        searchedFootballMatch.MaxNumberOfPlayers = footballMatch.MaxNumberOfPlayers;
        searchedFootballMatch.Date = footballMatch.Date;
        searchedFootballMatch.FootballPitchId = footballMatch.FootballPitchId;
    }

    public async Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.FootballMatches
            .Include(fm => fm.Creator)
            .Select(fm => new { Id = fm.Id, CreatorId = fm.Creator.Id })
            .AsNoTracking()
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId, cancellationToken);

        return result.CreatorId;
    }
}
