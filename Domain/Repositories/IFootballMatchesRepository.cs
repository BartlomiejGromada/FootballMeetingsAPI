using Domain.Entities;

namespace Domain.Repositories;

public interface IFootballMatchesRepository
{
    Task<IEnumerable<FootballMatch>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FootballMatch>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
    Task<FootballMatch> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task AddAsync(FootballMatch footballMatch, CancellationToken cancellationToken = default);
    Task RemoveByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task UpdateAsync(int footballMatchId, FootballMatch footballMatch);
}
