namespace Contracts.Models.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public string UserNickName { get; set; }
    public string UserRoleName { get; set; }
}
