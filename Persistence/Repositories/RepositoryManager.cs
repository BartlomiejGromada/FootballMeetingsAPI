using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        //private readonly Lazy<IFootballMatchesPlayersRepository> _lazyFootballMatchesPlayersRepository;
        private readonly Lazy<IFootballPitchesRepository> _lazyFootballPitchesRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(FootballMeetingsDbContext dbContext)
        {
           //lazyFootballMatchesPlayersRepository = new Lazy<IFootballMatchesPlayersRepository>(() => new FootballMatchesPlayersRepository(dbContext));
            _lazyFootballPitchesRepository = new Lazy<IFootballPitchesRepository>(() => new FootballPitchesRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IFootballMatchesPlayersRepository FootballMatchesPlayersRepository => throw new NotImplementedException();
        public IFootballPitchesRepository FootballPitchesRepository => _lazyFootballPitchesRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
