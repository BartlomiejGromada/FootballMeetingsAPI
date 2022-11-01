namespace Contracts.Models.FootballMatch;

public class ChangeOfPresenceDto
{
    public List<int> PlayersIds { get; set; }
    public bool WasPresent { get; set; }
}
