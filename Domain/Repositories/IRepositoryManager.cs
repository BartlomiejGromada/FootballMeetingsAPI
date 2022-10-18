namespace Domain.Repositories;

public interface IRepositoryManager
{
    IFootballMatchesPlayersRepository FootballMatchesPlayersRepository { get; }
    IFootballPitchesRepository FootballPitchesRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
