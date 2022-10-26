using Contracts.Models.FootballMatch;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

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
    public async Task<ActionResult<List<FootballMatchDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var footballMatches = await _footballMatchesService.GetAllAsync(cancellationToken);

        return Ok(footballMatches);
    }

    //[HttpGet("")]
    //public async Task<ActionResult<List<FootballMatchDto>>> GetAllByCreatorId(CancellationToken cancellationToken = default)
    //{
    //	var testId = 1;
    //	var footballMatches = await _footballMatchesService.GetAllByCreatorIdAsync(testId, cancellationToken);

    //	return Ok(footballMatches);
    //}

    [HttpGet("{id}")]
    public async Task<ActionResult<FootballMatchDto>> GetById([FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(id, cancellationToken);

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

        return Created($"api/v1/football-match/{footballMatchId}", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, UpdateFootballMatchDto dto,
        [FromServices] IValidator<UpdateFootballMatchDto> validator, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(id, cancellationToken);
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

        await _footballMatchesService.Update(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var footballMatch = await _footballMatchesService.GetByIdAsync(id, cancellationToken);
        if(_userContextService.GetUserRole != "Admin" && footballMatch.Creator.Id != _userContextService.GetUserId)
        {
            return new ForbidResult();
        }

        await _footballMatchesService.RemoveById(id);

        return NoContent();
    }
}
