using Domain.Entities;

namespace Domain.Repositories;

public interface IFootballMatchesPlayersRepository
{
    Task<IEnumerable<FootballMatchPlayer>> GetAllByFootballMatchIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task SignUpForMatch(int footballMatchId, int playerId);
    Task SignOffFromMatch(int footballMatchId, int playerId);
}
