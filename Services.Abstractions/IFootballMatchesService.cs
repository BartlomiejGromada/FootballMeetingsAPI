using Contracts.Models;
using Contracts.Models.FootballMatch;
using Domain.Entities;
using Sieve.Models;

namespace Services.Abstractions;

public interface IFootballMatchesService
{
    Task<PagedResult<FootballMatchDto>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default);
    Task<PagedResult<FootballMatchDto>> GetAllByCreatorIdAsync(SieveModel query, int creatorId, CancellationToken cancellationToken = default);
    Task<FootballMatchDto> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<int> Add(AddFootballMatchDto dto);
    Task RemoveById(int footballMatchId);
    Task Update(int footballMatchId, UpdateFootballMatchDto dto);
    Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task SingUpForMatch(int footballMatchId, int playerId);
    Task SignOffFromMatch(int footballMatchId, int playerId);
}
