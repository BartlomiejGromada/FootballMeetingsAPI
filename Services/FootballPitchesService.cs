using AutoMapper;
using Contracts.FootballPitch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public sealed class FootballPitchesService : IFootballPitchesService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public FootballPitchesService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<List<FootballPitchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var footballPitches = await _repositoryManager.FootballPitchesRepository
            .GetAllAsync(cancellationToken);

        return _mapper.Map<List<FootballPitchDto>>(footballPitches);
    }

    public async Task<FootballPitchDto> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default)
    {
       var footballPitch = await _repositoryManager.FootballPitchesRepository
            .GetByIdAsync(footballPitchId, cancellationToken);
    
       if(footballPitch is null)
        {
            throw new NotFoundException($"Football pitch with id {footballPitchId} cannot be found");
        }
    
        return _mapper.Map<FootballPitchDto>(footballPitch);
    }

    public async Task<FootballPitchDto> AddAsync(AddFootballPitchDto dto, CancellationToken cancellationToken = default)
    {
        var footballPitch = _mapper.Map<FootballPitch>(dto);

        await _repositoryManager.FootballPitchesRepository
            .AddAsync(footballPitch, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FootballPitchDto>(footballPitch);
    }

    public async Task RemoveById(int footballPitchId, CancellationToken cancellationToken = default)
    {
        var footballPitchDto = await GetByIdAsync(footballPitchId, cancellationToken);

        _repositoryManager.FootballPitchesRepository
            .Remove(_mapper.Map<FootballPitch>(footballPitchDto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(int footballPiatchId, UpdateFootballPitchDto dto, CancellationToken cancellationToken = default)
    {
        var footballPitchDto = await GetByIdAsync(footballPiatchId, cancellationToken);

        await _repositoryManager.FootballPitchesRepository
            .UpdateAsync(footballPiatchId, _mapper.Map<FootballPitch>(footballPitchDto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
