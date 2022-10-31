using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Persistence.Repositories;

internal sealed class FootballMatchesRepository : IFootballMatchesRepository
{
    private readonly FootballMeetingsDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public FootballMatchesRepository(FootballMeetingsDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<IEnumerable<FootballMatch>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default)
    {
        return await _sieveProcessor
            .Apply(query, _dbContext.FootballMatches
                .Include(fm => fm.Players)
                .Include(fm => fm.FootballPitch)
                .Include(fm => fm.Creator))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FootballMatch>> GetAllByCreatorIdAsync(SieveModel query, int creatorId, CancellationToken cancellationToken = default)
    {
        return await _sieveProcessor
            .Apply(query, _dbContext.FootballMatches
                .Include(fm => fm.Players)
                .Include(fm => fm.FootballPitch)
                .Include(fm => fm.Creator))
            .Where(fm => fm.CreatorId == creatorId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(SieveModel query, int? creatorId = null, CancellationToken cancellationToken = default)
    {
        if(creatorId == null)
        {
            return await _sieveProcessor
                .Apply(query, _dbContext.FootballMatches, applyFiltering: false, applySorting: false)
                .CountAsync();
        }

        return await _sieveProcessor
            .Apply(query, _dbContext.FootballMatches, applyFiltering: false, applySorting: false)
            .Where(fm => fm.CreatorId == creatorId.Value)
            .CountAsync();
    }

    public async Task<FootballMatch> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .Include(fm => fm.FootballPitch)
            .Include(fm => fm.Creator)
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

    public async Task SignUpForMatch(int footballMatchId, int playerId)
    {
        var footballMatch = await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId);

        var newPlayer = new User() { Id = playerId };
        _dbContext.Entry(newPlayer).State = EntityState.Unchanged;
        footballMatch.Players.Add(newPlayer);
    }

    public async Task SignOffFromMatch(int footballMatchId, int playerId)
    {
        var footballMatch = await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId);

        var player = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == playerId);

        footballMatch.Players.Remove(player);
    }

    public async Task<bool> ExistsAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .AnyAsync(fm => fm.Id == footballMatchId, cancellationToken);
    }

    public async Task<bool> ExistsPlayerInFootballMatchAsync(int footballMatchId, int playerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .Select(fm => new
            {
                fm.Id,
                fm.Players
            })
            .AnyAsync(item => item.Id == footballMatchId && item.Players.Any(player => player.Id == playerId), cancellationToken);
    }

    public async Task DeletePlayerFromMatch(int footballMatchId, int playerId)
    {
        var footballMatch = await _dbContext.FootballMatches
            .Include(fm => fm.Players)
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId);

        var playerToDelete = footballMatch.Players.FirstOrDefault(player => player.Id == playerId);

        footballMatch.Players.Remove(playerToDelete);
    }

    public async Task<int?> GetMaxNumberOfPlayersAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.FootballMatches
            .Select(fm => new { fm.Id, fm.MaxNumberOfPlayers })
            .FirstOrDefaultAsync(fm => fm.Id == footballMatchId, cancellationToken);

        return result.MaxNumberOfPlayers;
    }
}
