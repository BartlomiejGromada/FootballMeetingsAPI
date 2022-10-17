namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NickName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public virtual Role Role { get; set; }
    public int RoleId { get; set; }
    public virtual ICollection<FootballMatch> FootballMatches { get; set; }
}
