using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers.v1;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v1/football-match/{footballMatchId}/players")]
public class FootballMatchesPlayersController : ControllerBase
{
    private readonly IFootballMatchesPlayersService _footballMatchesPlayersService;
    public FootballMatchesPlayersController(IFootballMatchesPlayersService footballMatchesPlayersService)
    {
        _footballMatchesPlayersService = footballMatchesPlayersService;
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReportFromMatch([FromRoute] int footballMatchId)
    {
        var (fileName, contentFile) = await _footballMatchesPlayersService.GetReporstFromMatch(footballMatchId);

        return File(contentFile, @"application/vnd.ms-excel", fileName);
    }

    [HttpPost]
    public async Task<ActionResult> SingUpForMatch([FromRoute] int footballMatchId, [FromQuery] int playerId)
    {
        await _footballMatchesPlayersService.SingUpForMatch(footballMatchId, playerId);

        return Ok();
    }

    [HttpDelete("{playerId}")]
    public async Task<ActionResult> SignOffFromMatch([FromRoute] int footballMatchId, [FromRoute] int playerId)
    {
        await _footballMatchesPlayersService.SignOffFromMatch(footballMatchId, playerId);

        return Ok();
    }

    [HttpPatch]
    [Authorize(Roles = "Admin, Creator")]
    public async Task<ActionResult> ChangeOfPresence([FromRoute] int footballMatchId, [FromQuery] List<int> playersIds, [FromBody] JsonPatchDocument dto)
    {
        await _footballMatchesPlayersService.ChangeOfPresence(footballMatchId, playersIds, dto);

        return NoContent();
    }
}
