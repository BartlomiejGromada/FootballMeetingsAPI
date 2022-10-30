using Contracts.Models.Comments;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers.v1;

[ApiController]
[Produces("application/json")]
[Route("api/v1/football-matches/{footballMatchId}/comments")]
[ApiVersion("1.0")]
public class CommentsController : ControllerBase
{
    private readonly ICommentsService _commentsService;

	public CommentsController(ICommentsService commentsService)
	{
        _commentsService = commentsService;
	}

	[HttpGet]
	public async Task<ActionResult<List<CommentDto>>> GetAll([FromRoute] int footballMatchId, CancellationToken cancellationToken = default)
	{
		var comments = await _commentsService.GetAllAsync(footballMatchId, cancellationToken);

		return Ok(comments);
	}

	[HttpGet("{commentId}")]
	public async Task<ActionResult<CommentDto>> GetById([FromRoute] int footballMatchId, [FromRoute] int commentId,
		CancellationToken cancellationToken = default)
	{
		var comment = await _commentsService.GetByIdAsync(footballMatchId, commentId, cancellationToken);

		return Ok(comment);
	}

	[HttpPost]
	public async Task<ActionResult> Add([FromRoute] int footballMatchId, [FromBody] AddCommentDto dto)
	{
		var commentId = await _commentsService.Add(footballMatchId, dto);

		return CreatedAtAction(nameof(GetById), new 
		{
			footballMatchId, 
			commentId 
		}, null);
	}

	[HttpPut("{commentId}")]
	public async Task<ActionResult> Update([FromRoute] int footballMatchId, [FromRoute] int commentId, [FromBody] UpdateCommentDto dto)
	{
		await _commentsService.Update(footballMatchId, commentId, dto);

		return NoContent();
	}

	[HttpDelete("{commentId}")]
	public async Task<ActionResult> Delete([FromRoute] int footballMatchId, [FromRoute] int commentId)
	{
		await _commentsService.Delete(footballMatchId, commentId);

		return NoContent();
	}
}
