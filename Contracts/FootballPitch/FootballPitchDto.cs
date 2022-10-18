namespace Contracts.FootballPitch;

public class FootballPitchDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public byte[] Image { get; set; }

    public List<FootballMatchDto> FootballMatches { get; set; }
}
