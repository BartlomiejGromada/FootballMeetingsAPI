using AutoMapper;
using Contracts.FootballMatch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public class FootballMatchesService : IFootballMatchesService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public FootballMatchesService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<List<FootballMatchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var footballMatches = await _repositoryManager.FootballMatchesRepository
            .GetAllAsync(cancellationToken);

        return _mapper.Map<List<FootballMatchDto>>(footballMatches);
    }

    public async Task<List<FootballMatchDto>> GetAllByCreatorIdAsync(int creatorId, CancellationToken cancellationToken = default)
    {
        var footballMatches = await _repositoryManager.FootballMatchesRepository
            .GetAllByCreatorIdAsync(creatorId, cancellationToken);

        return _mapper.Map<List<FootballMatchDto>>(footballMatches);
    }

    public async Task<FootballMatchDto> GetByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _repositoryManager.FootballMatchesRepository
            .GetByIdAsync(footballMatchId, cancellationToken);

        if(footballMatch is null)
        {
            throw new NotFoundException($"Football match with id {footballMatchId} cannot be found");
        }

        return _mapper.Map<FootballMatchDto>(footballMatch);
    }

    public async Task<FootballMatchDto> AddAsync(AddFootballMatchDto dto, CancellationToken cancellationToken = default)
    {
        var footballMatch = _mapper.Map<FootballMatch>(dto);

        await _repositoryManager.FootballMatchesRepository
            .AddAsync(footballMatch, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FootballMatchDto>(cancellationToken);
    }

    public async Task RemoveByIdAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        await GetByIdAsync(footballMatchId, cancellationToken);

        await _repositoryManager.FootballMatchesRepository.RemoveByIdAsync(footballMatchId, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(int footballMatchId, UpdateFootballMatchDto dto, CancellationToken cancellationToken = default)
    {
        var footballMatchDto = await GetByIdAsync(footballMatchId, cancellationToken);

        await _repositoryManager.FootballMatchesRepository
            .UpdateAsync(footballMatchId, _mapper.Map<FootballMatch>(footballMatchDto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
