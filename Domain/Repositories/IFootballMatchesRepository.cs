﻿using Domain.Entities;
using Sieve.Models;

namespace Domain.Repositories;

public interface IFootballMatchesRepository
{
    Task<IEnumerable<FootballMatch>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default);
    Task<IEnumerable<FootballMatch>> GetAllByCreatorIdAsync(SieveModel query, int creatorId, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(SieveModel query, int? creatorId = null, CancellationToken cancellationToken = default);
    Task<FootballMatch> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task Add(FootballMatch footballMatch);
    Task RemoveById(int footballMatchId);
    Task Update(int footballMatchId, FootballMatch footballMatch);
    Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<bool> ExistsPlayerInFootballMatchAsync(int footballMatchId, int playerId, CancellationToken cancellationToken = default);
    //Task DeletePlayerFromMatch(int footballMatchId, int playerId);
    Task<int?> GetMaxNumberOfPlayersAsync(int footballMatchId, CancellationToken cancellationToken = default);
}
