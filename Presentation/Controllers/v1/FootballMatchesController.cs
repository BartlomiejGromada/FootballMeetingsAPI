using Contracts.Models.FootballMatch;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Sieve.Models;

namespace Presentation.Controllers.v1;

[ApiController]
[Route("api/v1/football-matches")]
[Produces("application/json")]
[ApiVersion("1.0")]
[Authorize]
public class FootballMatchesController : ControllerBase
{
    private readonly IFootballMatchesService _footballMatchesService;
    private readonly IUserContextService _userContextService;

    public FootballMatchesController(IFootballMatchesService footballMatchesService, IUserContextService userContextService)
    {
        _footballMatchesService = footballMatchesService;
        _userContextService = userContextService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FootballMatchDto>>> GetAll(SieveModel query, CancellationToken cancellationToken = default)
    {
        var footballMatches = await _footballMatchesService.GetAllAsync(query, cancellationToken);

        return Ok(footballMatches);
    }

    [HttpGet("{footballMatchId}")]
    public async Task<ActionResult<FootballMatchDto>> GetById([FromRoute] int footballMatchId,
        CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(footballMatchId, cancellationToken);

        return Ok(footballMatch);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Creator")]
    public async Task<ActionResult> Add([FromBody] AddFootballMatchDto dto,
        [FromServices] IValidator<AddFootballMatchDto> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var footballMatchId = await _footballMatchesService.Add(dto);

        return CreatedAtAction(nameof(GetById), new { footballMatchId }, null);
    }

    [HttpPut("{footballMatchId}")]
    public async Task<ActionResult> Update([FromRoute] int footballMatchId, UpdateFootballMatchDto dto,
        [FromServices] IValidator<UpdateFootballMatchDto> validator, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(footballMatchId, cancellationToken);
        if(_userContextService.GetUserRole != "Admin" && footballMatch.Creator.Id != _userContextService.GetUserId)
        {
            return new ForbidResult();
        }

        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _footballMatchesService.Update(footballMatchId, dto);

        return NoContent();
    }

    [HttpDelete("{footballMatchId}")]
    public async Task<ActionResult> RemoveById([FromRoute] int footballMatchId, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(footballMatchId, cancellationToken);
        if(_userContextService.GetUserRole != "Admin" && footballMatch.Creator.Id != _userContextService.GetUserId)
        {
            return new ForbidResult();
        }

        await _footballMatchesService.RemoveById(footballMatchId);

        return NoContent();
    }

    //[HttpPost("{footballMatchId}/players/{playerId}")]
    //public async Task<ActionResult> SingUpForMatch([FromRoute] int footballMatchId, [FromRoute] int playerId)
    //{
    //    await _footballMatchesService.SingUpForMatch(footballMatchId, playerId);

    //    return Ok();
    //}

    //[HttpDelete("{footballMatchId}/players/{playerId}")]
    //public async Task<ActionResult> SignOffFromMatch([FromRoute] int footballMatchId, [FromRoute] int playerId)
    //{
    //    await _footballMatchesService.SignOffFromMatch(footballMatchId, playerId);

    //    return Ok();
    //}

    //[HttpPatch("{footballMatchId}/players")]
    //[Authorize(Roles = "Admin, Creator")]
    //public async Task<ActionResult> ChangeOfPresence([FromRoute] int footballMatchId, [FromQuery] List<int> playersIds , [FromBody] JsonPatchDocument dto)
    //{
    //    await _footballMatchesService.ChangeOfPresence(footballMatchId, playersIds, dto);

    //    return NoContent();
    //}
}
