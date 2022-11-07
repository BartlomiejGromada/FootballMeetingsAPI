using Contracts.Models;
using Contracts.Models.FootballPitch;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers.v1;
using Services.Abstractions;
using Sieve.Models;
using Sieve.Services;
using System.Collections;

namespace FootballMeetingsAPITests.Controllers;

public class FootballPitchesControllerTests
{
    private readonly Mock<IFootballPitchesService> _mockFootballPitchesService;
    private readonly Mock<ISieveProcessor> _mockSieveProcessor;
    private readonly FootballPitchesController _footballPitchesController;

    public FootballPitchesControllerTests()
    {
        _mockFootballPitchesService = new Mock<IFootballPitchesService>();
        _mockSieveProcessor = new Mock<ISieveProcessor>();
        _footballPitchesController = new FootballPitchesController(_mockFootballPitchesService.Object, _mockSieveProcessor.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnResponnseOk()
    {
        //Arrange
        var query = new SieveModel();

        //Act
        var objectResult = await _footballPitchesController.GetAll(query);

        //Assert
        Assert.IsType<OkObjectResult>(objectResult.Result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnFootballPitches()
    {
        //Arrange
        var footballPitchFirst = new FootballPitchDto()
        {
            Id = 1,
            Name = "Football pitch 1",
            City = "Kalisz",
            Street = "Kochanowskiego",
            StreetNumber = "10"
        };
        var footballPitchSecond = new FootballPitchDto()
        {
            Id = 2,
            Name = "Football pitch 2",
            City = "OStrów Wielkopolski",
            Street = "Prosta",
            StreetNumber = "2"
        };
        var footballPitches = new List<FootballPitchDto>() { footballPitchFirst, footballPitchSecond };
        var query = new SieveModel();
        var pagedResult = new PagedResult<FootballPitchDto>(footballPitches, 1, 1, 1);

        _mockFootballPitchesService.Setup(m => m.GetAllAsync(query, default))
            .ReturnsAsync(pagedResult);

        //Act
        var objectResult = await _footballPitchesController.GetAll(query, default);
        var result = objectResult.Result as OkObjectResult;
        var value = result.Value as PagedResult<FootballPitchDto>;

        //Assert
        Assert.Equal(footballPitches.Count, value.Items.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnFootballPitch_WhenFootballPitchExists()
    {
        //Arrange
        var footballPitch = new FootballPitchDto()
        {
            Id = 1,
            Name = "Football pitch 1",
            City = "Kalisz",
            Street = "Kochanowskiego",
            StreetNumber = "10"
        };

        _mockFootballPitchesService.Setup(m => m.GetByIdAsync(footballPitch.Id, default))
            .ReturnsAsync(footballPitch);

        //Act
        var objectResult = await _footballPitchesController.GetById(footballPitch.Id, default);
        var result = objectResult.Result as OkObjectResult;
        var value = result.Value as FootballPitchDto;

        //Assert
        Assert.IsType<OkObjectResult>(result);
        _mockFootballPitchesService.Verify(m => m.GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        Assert.Equal(footballPitch.Id, value.Id);
        Assert.Equal(footballPitch.Name, value.Name);
        Assert.Equal(footballPitch.City, value.City);
        Assert.Equal(footballPitch.Street, value.Street);
        Assert.Equal(footballPitch.StreetNumber, value.StreetNumber);
    }

    [Fact]
    public async Task Add_ShouldReturnCreated_WhenDtoIsValid()
    {
        //Arrange
        var addFootballPitchDto = new Mock<AddFootballPitchDto>().Object;

        var exceptedValidationResult = new ValidationResult();

        var validator = new Mock<IValidator<AddFootballPitchDto>>();
        validator.Setup(m => m.ValidateAsync(addFootballPitchDto, default))
            .ReturnsAsync(() => exceptedValidationResult);

        //Act
        var objectResult = await _footballPitchesController.Add(addFootballPitchDto, null, validator.Object) as CreatedAtActionResult;

        //Assert
        Assert.IsType<CreatedAtActionResult>(objectResult);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
    }

    [Fact]
    public async Task Add_ShouldReturnBadRequest_WhenDtoIsInvalid()
    {
        //Arrange
        var addFootballPitchDto = new Mock<AddFootballPitchDto>().Object;

        var exceptedValidationErrors = new List<ValidationFailure>()
        {
            new ValidationFailure(nameof(AddFootballPitchDto.Name), "Error")
        };
        var exceptedValidationResult = new ValidationResult(exceptedValidationErrors);

        var validator = new Mock<IValidator<AddFootballPitchDto>>();
        validator.Setup(m => m.ValidateAsync(addFootballPitchDto, default))
            .ReturnsAsync(() => exceptedValidationResult);

        //Act
        var objectResult = await _footballPitchesController.Add(addFootballPitchDto, null, validator.Object) as BadRequestObjectResult;
        var actualValidationResult = (ICollection)objectResult.Value;

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.IsType<BadRequestObjectResult>(objectResult);
        Assert.Equal(exceptedValidationErrors.Count, actualValidationResult.Count);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenDtoIsValid()
    {
        //Arrange
        var updateFootballPitchDto = new Mock<UpdateFootballPitchDto>().Object;

        var exceptedValidationResult = new ValidationResult();

        var validator = new Mock<IValidator<UpdateFootballPitchDto>>();
        validator.Setup(m => m.Validate(updateFootballPitchDto))
            .Returns(() => exceptedValidationResult);

        //Act
        var objectResult = await _footballPitchesController.Update(It.IsAny<int>(), updateFootballPitchDto, null, validator.Object) as NoContentResult;

        //Assert
        Assert.IsType<NoContentResult>(objectResult);
        Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenDtoIsInvalid()
    {
        //Arrange
        var updateFootballPitchDto = new Mock<UpdateFootballPitchDto>().Object;

        var exceptedValidationErrors = new List<ValidationFailure>()
        {
            new ValidationFailure(nameof(UpdateFootballPitchDto.Name), "Error")
        };
        var exceptedValidationResult = new ValidationResult(exceptedValidationErrors);

        var validator = new Mock<IValidator<UpdateFootballPitchDto>>();
        validator.Setup(m => m.Validate(updateFootballPitchDto))
            .Returns(() => exceptedValidationResult);

        //Act
        var objectResult = await _footballPitchesController.Update(It.IsAny<int>(), updateFootballPitchDto, null, validator.Object) as BadRequestObjectResult;
        var actualValidationResult = (ICollection)objectResult.Value;

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.IsType<BadRequestObjectResult>(objectResult);
        Assert.Equal(exceptedValidationErrors.Count, actualValidationResult.Count);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent()
    {
        //Act
        var objectResult = await _footballPitchesController.RemoveById(It.IsAny<int>()) as NoContentResult;

        //Assert
        Assert.IsType<NoContentResult>(objectResult);
        Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
    }
}
