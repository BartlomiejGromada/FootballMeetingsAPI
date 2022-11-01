namespace Domain.Repositories;

public interface IRepositoryManager
{
    IFootballMatchesRepository FootballMatchesRepository { get; }
    IFootballPitchesRepository FootballPitchesRepository { get; }
    IUsersRepository UsersRepository { get; }
    IFootballMatchesPlayersRepository FootballMatchesPlayersRepository {get; }
    IAccountsRepository AccountsRepository { get; }
    ICommentsRepository CommentsRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
