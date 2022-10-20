namespace Contracts.Models.FootballMatch;

public class UpdateFootballMatchDto
{
    public string Name { get; set; }
    public int? MaxNumberOfPlayers { get; set; }
    public DateTime Date { get; set; }
    public int FootballPitchId { get; set; }
    public List<int> PlayersIds { get; set; }
}
