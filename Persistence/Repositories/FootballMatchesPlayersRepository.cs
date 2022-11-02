using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FootballMatchesPlayersRepository : IFootballMatchesPlayersRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public FootballMatchesPlayersRepository(FootballMeetingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<FootballMatchPlayer>> GetAllByFootballMatchIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballMatchesPlayers
             .Include(fmp => fmp.Player)
             .Include(fmp => fmp.FootballMatch)
                .ThenInclude(fm => fm.FootballPitch)
             .Include(fmp => fmp.FootballMatch)
                .ThenInclude(fm => fm.Creator)
             .Where(fmp => fmp.FootballMatchId == footballMatchId)
             .ToListAsync(cancellationToken);
    }

    public async Task SignUpForMatch(int footballMatchId, int playerId)
    {
        var footballMatchPlayer = new FootballMatchPlayer()
        {
            FootballMatchId = footballMatchId,
            PlayerId = playerId,
        };

        await _dbContext.FootballMatchesPlayers.AddAsync(footballMatchPlayer);
    }

    public async Task SignOffFromMatch(int footballMatchId, int playerId)
    {
        var footballMatchPlayer = await _dbContext.FootballMatchesPlayers
            .FirstOrDefaultAsync(fmp => fmp.FootballMatchId == footballMatchId && fmp.PlayerId == playerId);

        _dbContext.FootballMatchesPlayers.Remove(footballMatchPlayer);
    }
}
