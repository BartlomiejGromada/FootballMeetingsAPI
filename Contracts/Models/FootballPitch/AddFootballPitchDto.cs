namespace Contracts.Models.FootballPitch;

public class AddFootballPitchDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public byte[] Image { get; set; }
}
