using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Persistence.Repositories;

internal sealed class FootballPitchesRepository : IFootballPitchesRepository
{
    private readonly FootballMeetingsDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public FootballPitchesRepository(FootballMeetingsDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<bool> ExistsByIdAsync(int footballPitchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballPitches
            .AnyAsync(footballPitch => footballPitch.Id == footballPitchId, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string footballPitchName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballPitches
          .AnyAsync(footballPitch => footballPitch.Name.ToLower() == footballPitchName.ToLower().Trim(), cancellationToken);
    }

    public async Task<IEnumerable<FootballPitch>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default)
    {
        return await _sieveProcessor
            .Apply(query, _dbContext.FootballPitches)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(SieveModel query, CancellationToken cancellationToken = default)
    {
        return await _sieveProcessor
            .Apply(query, _dbContext.FootballPitches, applyFiltering: false, applySorting: false)
            .CountAsync(cancellationToken);
    }

    public async Task<FootballPitch> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballPitches
            .FirstOrDefaultAsync(footballPitch => footballPitch.Id == footballPitchId, cancellationToken);
    }

    public async Task<FootballPitch> GetByNameAsync(string footballPitchName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballPitches
            .FirstOrDefaultAsync(fp => fp.Name.ToLower() == footballPitchName.ToLower().Trim(), cancellationToken);
    }

    public async Task Add(FootballPitch footballPitch)
    {
        await _dbContext.FootballPitches.AddAsync(footballPitch);
    }

    public void Remove(FootballPitch footballPitch)
    {
        _dbContext.FootballPitches.Remove(footballPitch);
    }

    public async Task Update(int footballPiatchId, FootballPitch footballPitch)
    {
        var searchedFootballPitch = await _dbContext.FootballPitches
            .FirstOrDefaultAsync(pitch => pitch.Id == footballPiatchId);

        searchedFootballPitch.Name = footballPitch.Name;
        searchedFootballPitch.City = footballPitch.City;
        searchedFootballPitch.Street = footballPitch.Street;
        searchedFootballPitch.StreetNumber = footballPitch.StreetNumber;
        searchedFootballPitch.Image = footballPitch.Image;
    }
}
