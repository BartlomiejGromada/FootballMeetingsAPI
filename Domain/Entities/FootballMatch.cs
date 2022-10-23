namespace Domain.Entities;

public class FootballMatch
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? MaxNumberOfPlayers { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime Date { get; set; }

    public virtual ICollection<User> Players { get; set; } = new List<User>();
    public virtual FootballPitch FootballPitch { get; set; }
    public int FootballPitchId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual User Creator { get; set; }
    public int CreatorId { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
