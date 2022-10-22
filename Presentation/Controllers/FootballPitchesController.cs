using Contracts.Models.FootballPitch;
using Contracts.Validators;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v1/football-pitches")]
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

    [HttpGet("{id}")]
    public async Task<ActionResult<FootballPitchDto>> GetById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var footballPitch = await _footballPitchesService.GetByIdAsync(id, cancellationToken);

        return Ok(footballPitch);
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddFootballPitchDto dto, [FromServices] IValidator<AddFootballPitchDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if(!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return BadRequest(this.ModelState);
        }

        var footballPitchId = await _footballPitchesService.AddAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = footballPitchId }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, UpdateFootballPitchDto dto, 
        [FromServices] IValidator<UpdateFootballPitchDto> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(dto);
        try
        {
            await _footballPitchesService.UpdateAsync(id, dto, cancellationToken);
        }
        catch(FootballPitchNameIsAlreadyTakenException exception)
        {
            validationResult.Errors.Add(new ValidationFailure(nameof(UpdateFootballPitchDto.Name), exception.Message));
        }

        if(!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return BadRequest(this.ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveById([FromRoute]int id, CancellationToken cancellationToken = default)
    {
        await _footballPitchesService.RemoveByIdAsync(id, cancellationToken);

        return NoContent();
    }
}
