namespace Domain.Entities;

public class FootballPitch
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public byte[] Image { get; set; }

    public ICollection<FootballMatch> FootballMatches { get; set; }
}
