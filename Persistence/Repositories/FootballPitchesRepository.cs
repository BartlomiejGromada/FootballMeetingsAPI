using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal sealed class FootballPitchesRepository : IFootballPitchesRepository
    {
        private readonly FootballMeetingsDbContext _dbContext;

        public FootballPitchesRepository(FootballMeetingsDbContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<FootballPitch>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.FootballPitches
                .AsNoTracking()
                .ToListAsync(cancellationToken); 
        }

        public async Task<FootballPitch> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.FootballPitches
                .AsNoTracking()
                .FirstOrDefaultAsync(footballPitch => footballPitch.Id == footballPitchId, cancellationToken);
        }

        public async Task AddAsync(FootballPitch footballPitch, CancellationToken cancellationToken = default)
        {
            await _dbContext.FootballPitches.AddAsync(footballPitch, cancellationToken);
        }

        public void Remove(FootballPitch footballPitch)
        {
            _dbContext.FootballPitches.Remove(footballPitch);
        }

        public async Task UpdateAsync(int footballPiatchId, FootballPitch footballPitch)
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
}
