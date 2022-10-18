namespace Domain.Entities;

public class FootballMatch
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? MaxNumberOfPlayers { get; set; }
    public bool IsActive { get; set; }
    public DateTime Date { get; set; }

    public virtual ICollection<User> Players { get; set; }
    public virtual FootballPitch FootballPitch { get; set; }
    public int FootballPitchId { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual User Creator { get; set; }
    public int CreatorId { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
}
