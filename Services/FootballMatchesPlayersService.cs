using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Services.Abstractions;

namespace Services;

public class FootballMatchesPlayersService : IFootballMatchesPlayersService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IUserContextService _userContextService;

    public FootballMatchesPlayersService(IRepositoryManager repositoryManager, IUserContextService userContextService)
    {
        _repositoryManager = repositoryManager;
        _userContextService = userContextService;
    }

    public async Task SingUpForMatch(int footballMatchId, int playerId)
    {
        if (_userContextService.GetUserRole != "Admin" && _userContextService.GetUserRole != "Creator" &&
            _userContextService.GetUserId != playerId)
        {
            throw new ForbidException();
        }

        var footballMatch = await _repositoryManager.FootballMatchesRepository.GetByIdAsync(footballMatchId);
        if (footballMatch == null)
        {
            throw new NotFoundException($"Football match with id {footballMatchId} cannot be found");
        }

        var userExists = await _repositoryManager.UsersRepository.ExistsByIdAsync(playerId);
        if (!userExists)
        {
            throw new NotFoundException($"Player with id {playerId} cannot be found");
        }

        if (footballMatch.Date < DateTime.Now)
        {
            throw new MatchAlreadyTakenPlaceExpcetion($"Match with id {footballMatchId} already taken place {footballMatch.Date:dd-MM-yyyy}");
        }

        if (footballMatch.Players.Any(player => player.Id == playerId))
        {
            throw new PlayerIsAlreadySignedUpForMatchException($"Player with id {playerId} is already signed up for the match with id {footballMatchId}");
        }

        if (footballMatch.MaxNumberOfPlayers != null && footballMatch.MaxNumberOfPlayers.Value == footballMatch.Players.Count)
        {
            throw new MaxNumberOfPlayersExceededException($"Max number of players {footballMatch.MaxNumberOfPlayers.Value} exceeded ");
        }

        await _repositoryManager.FootballMatchesPlayersRepository.SignUpForMatch(footballMatchId, playerId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task SignOffFromMatch(int footballMatchId, int playerId)
    {
        if (_userContextService.GetUserRole != "Admin" && _userContextService.GetUserRole != "Creator" &&
            _userContextService.GetUserId != playerId)
        {
            throw new ForbidException();
        }

        var footballMatch = await _repositoryManager.FootballMatchesRepository.GetByIdAsync(footballMatchId);
        if (footballMatch == null)
        {
            throw new NotFoundException($"Football match with id {footballMatchId} cannot be found");
        }

        var userExists = await _repositoryManager.UsersRepository.ExistsByIdAsync(playerId);
        if (!userExists)
        {
            throw new NotFoundException($"Player with id {playerId} cannot be found");
        }

        if (footballMatch.Date < DateTime.Now)
        {
            throw new MatchAlreadyTakenPlaceExpcetion($"Match with id {footballMatchId} already taken place {footballMatch.Date:dd-MM-yyyy}");
        }

        if (footballMatch.Players.All(player => player.Id != playerId))
        {
            throw new PlayerHasNotSignedUpForMatchException($"Player with id {playerId} has not signed up for the match with id {footballMatchId}");
        }

        await _repositoryManager.FootballMatchesPlayersRepository.SignOffFromMatch(footballMatchId, playerId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task ChangeOfPresence(int footballMatchId, List<int> playersIds, JsonPatchDocument dto)
    {
        var footballMatch = await _repositoryManager.FootballMatchesRepository.GetByIdAsync(footballMatchId);
        if (footballMatch == null)
        {
            throw new NotFoundException($"Football match with id {footballMatchId} cannot be found");
        }

        if (footballMatch.Date > DateTime.Now)
        {
            throw new FootballMatchHasNotBeenPlayedYetException($"Football match with id {footballMatchId} has not been played yet");
        }

        var footballMatchPlayers = await _repositoryManager.FootballMatchesPlayersRepository.GetAllByFootballMatchIdAsync(footballMatchId);

        foreach (var footballMatchPlayer in footballMatchPlayers.ToList().Where(fmp => playersIds.Contains(fmp.PlayerId)))
        {
            dto.ApplyTo(footballMatchPlayer);
        }

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }
}
