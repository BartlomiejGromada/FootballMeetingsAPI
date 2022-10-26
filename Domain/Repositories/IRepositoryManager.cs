namespace Domain.Repositories;

public interface IRepositoryManager
{
    IFootballMatchesRepository FootballMatchesRepository { get; }
    IFootballPitchesRepository FootballPitchesRepository { get; }
    IUsersRepository UsersRepository { get; }
    IAccountRepository AccountRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
