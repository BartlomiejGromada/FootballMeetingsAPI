namespace Domain.Repositories;

public interface IRepositoryManager
{
    IFootballMatchesRepository FootballMatchesRepository { get; }
    IFootballPitchesRepository FootballPitchesRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
