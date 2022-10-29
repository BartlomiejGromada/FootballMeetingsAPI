using Domain.Entities;

namespace Domain.Repositories;

public interface ICommentsRepository
{
    Task<IEnumerable<Comment>> GetAllComments(int footballMatchId, CancellationToken cancellationToken = default);
    Task<Comment> GetByIdAsync(int commentId, CancellationToken cancellationToken = default);
    Task Add(Comment comment);
    Task<bool> IsCommentBelongsToUser(int commmentId, int userId);
    Task Update(int commentId, Comment comment);
    Task Delete(int commentId);
}
