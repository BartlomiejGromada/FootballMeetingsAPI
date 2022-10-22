using Contracts.Models.FootballPitch;

namespace Services.Abstractions;

public interface IFootballPitchesService
{
    Task<List<FootballPitchDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FootballPitchDto> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task<int> AddAsync(AddFootballPitchDto dto, CancellationToken cancellationToken = default);
    Task RemoveByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task UpdateAsync(int footballPiatchId, UpdateFootballPitchDto dto, CancellationToken cancellationToken = default);
}
