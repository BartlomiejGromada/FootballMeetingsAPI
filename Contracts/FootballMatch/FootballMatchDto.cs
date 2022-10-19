using Contracts.FootballPitch;

namespace Contracts.FootballMatch;

public class FootballMatchDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? MaxNumberOfPlayers { get; set; }
    public DateTime Date { get; set; }
    public List<PlayerDto> Players { get; set; }
    public FootballPitchDto FootballPitch { get; set; }
    public DateTime CreatedAt { get; set; }
    public PlayerDto Creator { get; set; }
    public List<CommentDto> Comments { get; set; }
}
