using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFootballPitchesRepository
    {
        Task<IEnumerable<FootballPitch>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<FootballPitch> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
        Task AddAsync(FootballPitch footballPitch);
        void Remove(FootballPitch footballPitch);
        Task UpdateAsync(int footballPiatchId, FootballPitch footballPitch);
    }
}