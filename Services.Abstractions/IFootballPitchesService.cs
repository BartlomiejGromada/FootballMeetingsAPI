using Contracts.Models;
using Contracts.Models.FootballPitch;
using Sieve.Models;

namespace Services.Abstractions;

public interface IFootballPitchesService
{
    Task<PagedResult<FootballPitchDto>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default);
    Task<FootballPitchDto> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
    Task<int> Add(AddFootballPitchDto dto);
    Task RemoveById(int footballPitchId);
    Task Update(int footballPiatchId, UpdateFootballPitchDto dto);
}
