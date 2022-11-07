using AutoMapper;
using Contracts.Models.FootballMatch;
using Contracts.Models.FootballPitch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using Services.Abstractions;
using Services.Services;

namespace FootballMeetingsAPITests.Services;

public class FootballPitchesServiceTests
{
    private readonly Mock<IRepositoryManager> _mockRepositoryManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IFootballPitchesService _footballPitchesService;

    public FootballPitchesServiceTests()
    {
        _mockRepositoryManager = new Mock<IRepositoryManager>();
        _mockMapper = new Mock<IMapper>();
        _footballPitchesService = new FootballPitchesService(_mockRepositoryManager.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_WhenFootballPitchExists()
    {
        //Arrange
        var footballPitch = new FootballPitch()
        {
            Id = 1,
            Name = "Football pitch 1",
            City = "Kalisz",
            Street = "Prosta",
            StreetNumber = "5"
        };

        var footballPitchDto = new FootballPitchDto()
        {
            Id = 1,
            Name = "Football pitch 1",
            City = "Kalisz",
            Street = "Prosta",
            StreetNumber = "5"
        };

        _mockRepositoryManager.Setup(m => m.FootballPitchesRepository.GetByIdAsync(footballPitch.Id, default))
            .ReturnsAsync(footballPitch);
        _mockMapper.Setup(x => x.Map<FootballPitchDto>(It.IsAny<FootballPitch>()))
            .Returns((FootballPitch source) =>
            {
                return footballPitchDto;
            });

        //Act
        var returnedFootballPitch = await _footballPitchesService.GetByIdAsync(footballPitch.Id, default);

        //Assert
        Assert.NotNull(returnedFootballPitch);
        Assert.Equal(footballPitchDto.Id, returnedFootballPitch.Id);
        Assert.Equal(footballPitchDto.Name, returnedFootballPitch.Name);
        Assert.Equal(footballPitchDto.City, returnedFootballPitch.City);
        Assert.Equal(footballPitchDto.Street, returnedFootballPitch.Street);
        Assert.Equal(footballPitchDto.StreetNumber, returnedFootballPitch.StreetNumber);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnThrowException_WhenFootballPitchNotExist()
    {
        //Arrange
        var footballPitch = new FootballPitch()
        {
            Id = 1,
            Name = "Football pitch 1",
            City = "Kalisz",
            Street = "Prosta",
            StreetNumber = "5"
        };

        _mockRepositoryManager.Setup(m => m.FootballPitchesRepository.GetByIdAsync(footballPitch.Id, default))
            .ReturnsAsync(() => null);

        //Act
        var action = () => _footballPitchesService.GetByIdAsync(footballPitch.Id, default);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(action);
        Assert.Equal($"Football pitch with id {footballPitch.Id} cannot be found", exception.Message);
    }

    [Fact]
    public async Task Add_ShouldReturnAddedFootballPitchId()
    {
        //Arrange
        var footballPitchId = 1;
        var dto = new AddFootballPitchDto();
        var addedFootballPitch = new FootballPitch();

        _mockMapper.Setup(x => x.Map<FootballPitch>(It.IsAny<AddFootballPitchDto>()))
         .Returns((AddFootballPitchDto source) =>
         {
             return addedFootballPitch;
         });
        _mockRepositoryManager.Setup(m => m.FootballPitchesRepository.Add(It.IsAny<FootballPitch>()));
        _mockRepositoryManager.Setup(m => m.UnitOfWork.SaveChangesAsync(default))
            .Callback(() => addedFootballPitch.Id = footballPitchId);

        //Act
        var addedFootballPitchId = await _footballPitchesService.Add(dto);

        //Assert
        Assert.Equal(footballPitchId, addedFootballPitchId);
        _mockRepositoryManager.Verify(m => m.FootballPitchesRepository.Add(It.IsAny<FootballPitch>()), Times.Once);
        _mockRepositoryManager.Verify(m => m.UnitOfWork.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenFootballPitchNameAlreadyExists()
    {
        //Arrange
        string footballPitchName = "Football pitch 1";

        var footballPitchToUpdate = new FootballPitch()
        {
            Id = 1,
            Name = footballPitchName
        };

        _mockRepositoryManager.Setup(m => m.FootballPitchesRepository.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(() => footballPitchToUpdate);
        _mockMapper.Setup(x => x.Map<FootballPitchDto>(It.IsAny<FootballPitch>()))
          .Returns((FootballPitch source) =>
          {
              return new FootballPitchDto()
              {
                  Id = footballPitchToUpdate.Id,
                  Name = footballPitchToUpdate.Name,
              };
          });
        _mockRepositoryManager.Setup(m => m.FootballPitchesRepository.GetByNameAsync(It.IsAny<string>(), default))
            .ReturnsAsync(() => new FootballPitch()
            {
                Id = 2,
                Name = footballPitchName,
            });

        //Act
        var action = () => _footballPitchesService.Update(footballPitchToUpdate.Id, new UpdateFootballPitchDto()
        {
            Name = footballPitchToUpdate.Name,
        });

        //Asserts
        var exception = await Assert.ThrowsAsync<FootballPitchNameIsAlreadyTakenException>(action);
        Assert.Equal($"Football pitch with name {footballPitchName} is already taken", exception.Message);
    }
}
