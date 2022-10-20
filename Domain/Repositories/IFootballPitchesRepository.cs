﻿using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFootballPitchesRepository
    {
        Task<IEnumerable<FootballPitch>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<FootballPitch> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default);
        Task<FootballPitch> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task AddAsync(FootballPitch footballPitch, CancellationToken cancellationToken = default);
        void Remove(FootballPitch footballPitch);
        Task UpdateAsync(int footballPiatchId, FootballPitch footballPitch);
    }
}