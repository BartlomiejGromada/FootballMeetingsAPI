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
}
