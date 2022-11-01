using Microsoft.AspNetCore.JsonPatch;

namespace Services.Abstractions;

public interface IFootballMatchesPlayersService
{
    Task SingUpForMatch(int footballMatchId, int playerId);
    Task SignOffFromMatch(int footballMatchId, int playerId);
    Task ChangeOfPresence(int footballMatchId, List<int> playersIds, JsonPatchDocument dto);
}
