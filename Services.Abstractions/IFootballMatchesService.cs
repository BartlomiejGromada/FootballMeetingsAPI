using Contracts.Models.FootballMatch;

namespace Services.Abstractions;

public interface IFootballMatchesService
{
    Task<List<FootballMatchDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<FootballMatchDto>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
    Task<FootballMatchDto> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<FootballMatchDto> AddAsync(AddFootballMatchDto dto, CancellationToken cancellationToken = default);
    Task RemoveByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task UpdateAsync(int footballMatchId, UpdateFootballMatchDto dto, CancellationToken cancellationToken = default);
}
