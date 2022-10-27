using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class FootballPitchesRepository : IFootballPitchesRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public FootballPitchesRepository(FootballMeetingsDbContext dbContext) => _dbContext = dbContext;

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

    public async Task<IEnumerable<FootballPitch>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.FootballPitches
            .AsNoTracking()
            .ToListAsync(cancellationToken); 
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
