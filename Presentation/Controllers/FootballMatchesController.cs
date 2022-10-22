using Contracts.Models.FootballMatch;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v1/football-matches")]
public class FootballMatchesController : ControllerBase
{
    private readonly IFootballMatchesService _footballMatchesService;

	public FootballMatchesController(IFootballMatchesService footballMatchesService)
	{
		_footballMatchesService = footballMatchesService;
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
	public async Task<ActionResult> Add([FromBody] AddFootballMatchDto dto, 
		[FromServices] IValidator<AddFootballMatchDto> validator, CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(dto, cancellationToken);
		if(!validationResult.IsValid)
		{
			validationResult.AddToModelState(this.ModelState);
			return BadRequest(this.ModelState);
		}

		var footballMatchId = await _footballMatchesService.AddAsync(dto, cancellationToken);

		return Created($"api/v1/football-match/{footballMatchId}", null);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Update([FromRoute] int id, UpdateFootballMatchDto dto,
		[FromServices] IValidator<UpdateFootballMatchDto> validator, CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(dto, cancellationToken);
		if(!validationResult.IsValid)
		{
			validationResult.AddToModelState(this.ModelState);
			return BadRequest(this.ModelState);
		}

		await _footballMatchesService.UpdateAsync(id, dto, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> RemoveById([FromRoute] int id, CancellationToken cancellationToken = default)
	{
		await _footballMatchesService.RemoveByIdAsync(id, cancellationToken);

		return NoContent();
	}
}
