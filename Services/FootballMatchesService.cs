using AutoMapper;
using Contracts.Models;
using Contracts.Models.FootballMatch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;
using Sieve.Models;

namespace Services;

public sealed class FootballMatchesService : IFootballMatchesService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public FootballMatchesService(IRepositoryManager repositoryManager, IMapper mapper, IUserContextService userContextService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<PagedResult<FootballMatchDto>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default)
    {
        var footballMatches = await _repositoryManager.FootballMatchesRepository
            .GetAllAsync(query, cancellationToken);

        var dtos = _mapper.Map<List<FootballMatchDto>>(footballMatches);

        var totalCount = await _repositoryManager.FootballMatchesRepository
         .GetCountAsync(query, null, cancellationToken);

        return new PagedResult<FootballMatchDto>(dtos, totalCount, query.PageSize.Value, query.Page.Value);
    }

    public async Task<PagedResult<FootballMatchDto>> GetAllByCreatorIdAsync(SieveModel query, int creatorId, CancellationToken cancellationToken = default)
    {
        var footballMatches = await _repositoryManager.FootballMatchesRepository
            .GetAllByCreatorIdAsync(query, creatorId, cancellationToken);

        var dtos = _mapper.Map<List<FootballMatchDto>>(footballMatches);

        var totalCount = await _repositoryManager.FootballMatchesRepository
         .GetCountAsync(query, creatorId, cancellationToken);

        return new PagedResult<FootballMatchDto>(dtos, totalCount, query.PageSize.Value, query.Page.Value);
    }

    public async Task<FootballMatchDto> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _repositoryManager.FootballMatchesRepository
            .GetByIdAsync(footballMatchId, cancellationToken);

        if (footballMatch == null)
        {
            throw new NotFoundException($"Football match with id {footballMatchId} cannot be found");
        }

        return _mapper.Map<FootballMatchDto>(footballMatch);
    }

    public async Task<int> Add(AddFootballMatchDto dto)
    {
        dto.PlayersIds = dto.PlayersIds
            .Distinct()
            .ToList();

        if (dto.MaxNumberOfPlayers != null && dto.MaxNumberOfPlayers.Value < dto.PlayersIds.Count)
        {
            throw new MaximumNumberOfPlayersExceededException($"The maximum number of players has been exceeded");
        }

        var footballMatch = _mapper.Map<FootballMatch>(dto);

        footballMatch.CreatorId = _userContextService.GetUserId;
        footballMatch.IsActive = true;
        footballMatch.CreatedAt = DateTime.Now;

        foreach (var playerId in dto.PlayersIds)
        {
            footballMatch.Players.Add(new User() { Id = playerId });
        }

        await _repositoryManager.FootballMatchesRepository
            .Add(footballMatch);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return footballMatch.Id;
    }

    public async Task RemoveById(int footballMatchId)
    {
        await _repositoryManager.FootballMatchesRepository.RemoveById(footballMatchId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task Update(int footballMatchId, UpdateFootballMatchDto dto)
    {
        var footballMatch = await _repositoryManager.FootballMatchesRepository.GetByIdAsync(footballMatchId);

        dto.PlayersIds = dto.PlayersIds
            .Where(id => !dto.PlayersIdsToDelete.Contains(id))
            .ToList();

        if (dto.MaxNumberOfPlayers != null && dto.MaxNumberOfPlayers.Value < (footballMatch.Players.Count + (dto.PlayersIds.Count - dto.PlayersIdsToDelete.Count)))
        {
            throw new MaximumNumberOfPlayersExceededException($"The maximum number of players has been exceeded");
        }

        #region Delete players from match
        foreach (var playerIdToDelete in dto.PlayersIdsToDelete)
        {
            var playerIsInMatch = footballMatch.Players.Any(player => dto.PlayersIdsToDelete.Contains(player.Id));
            if(playerIsInMatch)
            {
                await _repositoryManager.FootballMatchesRepository.DeletePlayerFromMatch(footballMatchId, playerIdToDelete);
            }
        }
        #endregion

        #region Add players to match
        foreach (var playerId in dto.PlayersIds)
        {
            var playerIsInMatch = footballMatch.Players.Any(player => dto.PlayersIds.Contains(player.Id));
            if (!playerIsInMatch)
            {
                await _repositoryManager.FootballMatchesRepository.SignUpForMatch(footballMatchId, playerId);
            }
        }
        #endregion

        await _repositoryManager.FootballMatchesRepository
                .Update(footballMatchId, _mapper.Map<FootballMatch>(dto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetCreatorIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _repositoryManager.FootballMatchesRepository
            .GetCreatorIdAsync(footballMatchId, cancellationToken);
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

        await _repositoryManager.FootballMatchesRepository.SignUpForMatch(footballMatchId, playerId);

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

        await _repositoryManager.FootballMatchesRepository.SignOffFromMatch(footballMatchId, playerId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }
}
