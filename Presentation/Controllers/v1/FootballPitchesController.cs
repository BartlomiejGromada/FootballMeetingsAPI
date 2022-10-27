using Contracts.Models.FootballPitch;
using Contracts.Validators;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers.v1;

[ApiController]
[Route("api/v1/football-pitches")]
[Produces("application/json")]
[ApiVersion("1.0")]
[Authorize]
public class FootballPitchesController : ControllerBase
{
    private readonly IFootballPitchesService _footballPitchesService;

    public FootballPitchesController(IFootballPitchesService footballPitchesService)
    {
        _footballPitchesService = footballPitchesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FootballPitchDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var footballPitches = await _footballPitchesService.GetAllAsync(cancellationToken);

        return Ok(footballPitches);
    }

    [HttpGet("{footballPitchId}")]
    public async Task<ActionResult<FootballPitchDto>> GetById([FromRoute] int footballPitchId, CancellationToken cancellationToken = default)
    {
        var footballPitch = await _footballPitchesService.GetByIdAsync(footballPitchId, cancellationToken);

        return Ok(footballPitch);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Creator")]
    public async Task<ActionResult> Add([FromBody] AddFootballPitchDto dto, [FromServices] IValidator<AddFootballPitchDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var footballPitchId = await _footballPitchesService.Add(dto);

        return CreatedAtAction(nameof(GetById), new { id = footballPitchId }, null);
    }

    [HttpPut("{footballPitchId}")]
    [Authorize(Roles = "Admin, Creator")]
    public async Task<ActionResult> Update([FromRoute] int footballPitchId, UpdateFootballPitchDto dto,
        [FromServices] IValidator<UpdateFootballPitchDto> validator)
    {
        var validationResult = validator.Validate(dto);
        try
        {
            await _footballPitchesService.Update(footballPitchId, dto);
        }
        catch (FootballPitchNameIsAlreadyTakenException exception)
        {
            validationResult.Errors.Add(new ValidationFailure(nameof(UpdateFootballPitchDto.Name), exception.Message));
        }

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin, Creator")]
    [HttpDelete("{footballPitchId}")]
    public async Task<ActionResult> RemoveById([FromRoute] int footballPitchId)
    {
        await _footballPitchesService.RemoveById(footballPitchId);

        return NoContent();
    }
}
