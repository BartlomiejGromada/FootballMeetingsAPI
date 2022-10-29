using Domain.Repositories;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IFootballMatchesRepository> _lazyFootballMatchesRepository;
    private readonly Lazy<IFootballPitchesRepository> _lazyFootballPitchesRepository;
    private readonly Lazy<IUsersRepository> _lazyUsersRepository;
    private readonly Lazy<IAccountsRepository> _lazyAccountsRepository;
    private readonly Lazy<ICommentsRepository> _lazyCommentsRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(FootballMeetingsDbContext dbContext)
    {
        _lazyFootballMatchesRepository = new Lazy<IFootballMatchesRepository>(() => new FootballMatchesRepository(dbContext));
        _lazyFootballPitchesRepository = new Lazy<IFootballPitchesRepository>(() => new FootballPitchesRepository(dbContext));
        _lazyUsersRepository = new Lazy<IUsersRepository>(() => new UsersRepository(dbContext));
        _lazyAccountsRepository = new Lazy<IAccountsRepository>(() => new AccountsRepository(dbContext));
        _lazyCommentsRepository = new Lazy<ICommentsRepository>(() => new CommentsRepository(dbContext));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
    }

    public IFootballMatchesRepository FootballMatchesRepository => _lazyFootballMatchesRepository.Value;
    public IFootballPitchesRepository FootballPitchesRepository => _lazyFootballPitchesRepository.Value;
    public IUsersRepository UsersRepository => _lazyUsersRepository.Value;
    public IAccountsRepository AccountsRepository => _lazyAccountsRepository.Value;
    public ICommentsRepository CommentsRepository => _lazyCommentsRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}
