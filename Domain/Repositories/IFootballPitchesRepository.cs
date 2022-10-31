using Domain.Entities;
using Sieve.Models;

namespace Domain.Repositories;

public interface IFootballPitchesRepository
{
    Task<bool> ExistsByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string footballPitchName, CancellationToken cancellationToken = default);
    Task<IEnumerable<FootballPitch>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(SieveModel query, CancellationToken cancellationToken = default);
    Task<FootballPitch> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task<FootballPitch> GetByNameAsync(string footballPitchName, CancellationToken cancellationToken = default);
    Task Add(FootballPitch footballPitch);
    void Remove(FootballPitch footballPitch);
    Task Update(int footballPiatchId, FootballPitch footballPitch);
}