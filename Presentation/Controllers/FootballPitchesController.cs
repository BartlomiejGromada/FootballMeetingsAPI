using Contracts.FootballPitch;
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
    public async Task<ActionResult<FootballPitchDto>> GetById([FromRoute] int id, 
        CancellationToken cancellationToken = default)
    {
        var footballPitch = await _footballPitchesService.GetByIdAsync(id, cancellationToken);

        return Ok(footballPitch);
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddFootballPitchDto dto, 
        CancellationToken cancellationToken = default)
    {
        var footballPitch = await _footballPitchesService.AddAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = footballPitch.Id }, footballPitch);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, UpdateFootballPitchDto dto, 
        CancellationToken cancellationToken = default)
    {
        await _footballPitchesService.UpdateAsync(id, dto, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveById([FromRoute]int id, CancellationToken cancellationToken = default)
    {
        await _footballPitchesService.RemoveById(id, cancellationToken);

        return NoContent();
    }
}
