using Domain.Entities;

namespace Domain.Repositories;

public interface IFootballMatchesPlayersRepository
{
    IEnumerable<FootballMatchPlayer> GetAllByCreatorIdAsync(int createdId, CancellationToken cancellationToken = default);
    IEnumerable<FootballMatchPlayer> GetAllAsync(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default);
}
