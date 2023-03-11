using AutoMapper;
using Contracts.Models;
using Contracts.Models.FootballPitch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;
using Sieve.Models;

namespace Services.Services;

public sealed class FootballPitchesService : IFootballPitchesService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public FootballPitchesService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<PagedResult<FootballPitchDto>> GetAllAsync(SieveModel query, CancellationToken cancellationToken = default)
    {
        var footballPitches = await _repositoryManager.FootballPitchesRepository
            .GetAllAsync(query, cancellationToken);

        var dtos = _mapper.Map<List<FootballPitchDto>>(footballPitches);

        return new PagedResult<FootballPitchDto>(dtos, query.PageSize.Value, query.Page.Value);
    }

    public async Task<FootballPitchDto> GetByIdAsync(int footballPitchId, CancellationToken cancellationToken = default)
    {
        var footballPitch = await _repositoryManager.FootballPitchesRepository
             .GetByIdAsync(footballPitchId, cancellationToken);

        if (footballPitch == null)
        {
            throw new NotFoundException($"Football pitch with id {footballPitchId} cannot be found");
        }

        return _mapper.Map<FootballPitchDto>(footballPitch);
    }

    public async Task<int> Add(AddFootballPitchDto dto)
    {
        var footballPitch = _mapper.Map<FootballPitch>(dto);

        await _repositoryManager.FootballPitchesRepository
            .Add(footballPitch);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return footballPitch.Id;
    }

    public async Task RemoveById(int footballPitchId)
    {
        var footballPitchDto = await GetByIdAsync(footballPitchId);

        _repositoryManager.FootballPitchesRepository
            .Remove(_mapper.Map<FootballPitch>(footballPitchDto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task Update(int footballPiatchId, UpdateFootballPitchDto dto)
    {
        var footballPitchDto = await GetByIdAsync(footballPiatchId);

        var footballPitchByName = await _repositoryManager.FootballPitchesRepository.GetByNameAsync(dto.Name);
        if (footballPitchByName != null && footballPitchDto.Id != footballPitchByName.Id)
        {
            throw new FootballPitchNameIsAlreadyTakenException($"Football pitch with name {dto.Name} is already taken");
        }

        await _repositoryManager.FootballPitchesRepository
            .Update(footballPiatchId, _mapper.Map<FootballPitch>(dto));

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }
}
