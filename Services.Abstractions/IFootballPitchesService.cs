using Contracts.Models.FootballPitch;

namespace Services.Abstractions;

public interface IFootballPitchesService
{
    Task<List<FootballPitchDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FootballPitchDto> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task<int> Add(AddFootballPitchDto dto);
    Task RemoveById(int footballPitchId);
    Task Update(int footballPiatchId, UpdateFootballPitchDto dto);
}
