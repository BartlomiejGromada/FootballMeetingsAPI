using Domain.Repositories;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IFootballMatchesRepository> _lazyFootballMatchesRepository;
    private readonly Lazy<IFootballPitchesRepository> _lazyFootballPitchesRepository;
    private readonly Lazy<IUsersRepository> _lazyUsersRepository;
    private readonly Lazy<IAccountRepository> _lazyAccountRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(FootballMeetingsDbContext dbContext)
    {
        _lazyFootballMatchesRepository = new Lazy<IFootballMatchesRepository>(() => new FootballMatchesRepository(dbContext));
        _lazyFootballPitchesRepository = new Lazy<IFootballPitchesRepository>(() => new FootballPitchesRepository(dbContext));
        _lazyUsersRepository = new Lazy<IUsersRepository>(() => new UsersRepository(dbContext));
        _lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(dbContext));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
    }

    public IFootballMatchesRepository FootballMatchesRepository => _lazyFootballMatchesRepository.Value;
    public IFootballPitchesRepository FootballPitchesRepository => _lazyFootballPitchesRepository.Value;
    public IUsersRepository UsersRepository => _lazyUsersRepository.Value;
    public IAccountRepository AccountRepository => _lazyAccountRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}
