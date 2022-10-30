using Contracts.Models.FootballPitch;
using Contracts.Validators;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public async Task<ActionResult> Add([FromForm] AddFootballPitchDto dto, IFormFile image,
        [FromServices] IValidator<AddFootballPitchDto> validator)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        if (image != null)
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            dto.Image = memoryStream.ToArray();
        }

        var footballPitchId = await _footballPitchesService.Add(dto);

        return CreatedAtAction(nameof(GetById), new { footballPitchId }, null);
    }

    [HttpPut("{footballPitchId}")]
    [Authorize(Roles = "Admin, Creator")]
    public async Task<ActionResult> Update([FromRoute] int footballPitchId, UpdateFootballPitchDto dto, IFormFile image,
        [FromServices] IValidator<UpdateFootballPitchDto> validator)
    {
        var validationResult = validator.Validate(dto);
        try
        {
            if (image != null)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                dto.Image = memoryStream.ToArray();
            }

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
