namespace Contracts.FootballMatch;

public class AddFootballMatchDto
{
    public string Name { get; set; }
    public int? MaxNumberOfPlayers { get; set; }
    public DateTime Date { get; set; }
    public int FootballPitchId { get; set; }
    public List<int> PlayersIds { get; set; }
}
