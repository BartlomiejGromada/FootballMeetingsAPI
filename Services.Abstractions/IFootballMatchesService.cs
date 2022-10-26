using Contracts.Models.FootballMatch;

namespace Services.Abstractions;

public interface IFootballMatchesService
{
    Task<List<FootballMatchDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<FootballMatchDto>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default);
    Task<FootballMatchDto> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<int> Add(AddFootballMatchDto dto);
    Task RemoveById(int footballMatchId);
    Task Update(int footballMatchId, UpdateFootballMatchDto dto);
    Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
}
