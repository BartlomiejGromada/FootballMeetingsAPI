using Domain.Entities;

namespace Domain.Repositories;

public interface IFootballMatchesRepository
{
    Task<IEnumerable<FootballMatch>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FootballMatch>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
    Task<FootballMatch> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task Add(FootballMatch footballMatch);
    Task RemoveById(int footballMatchId);
    Task Update(int footballMatchId, FootballMatch footballMatch);
    Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task SignUpForMatch(int footballMatchId, int playerId);
    Task SignOffFromMatch(int footballMatchId, int playerId);
    Task<bool> ExistsAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<bool> ExistsPlayerInFootballMatchAsync(int footballMatchId, int playerId, CancellationToken cancellationToken = default);
    Task DeletePlayerFromMatch(int footballMatchId, int playerId);
    Task<int?> GetMaxNumberOfPlayersAsync(int footballMatchId, CancellationToken cancellationToken = default);
}
