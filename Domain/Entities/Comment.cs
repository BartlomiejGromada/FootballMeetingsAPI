namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; }
    public int UserId { get; set; }
    public virtual FootballMatch FootballMatch { get; set; }
    public int FootballMatchId { get; set; }
}
