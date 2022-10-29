using Contracts.Models.Comments;

namespace Services.Abstractions;

public interface ICommentsService
{
    Task<List<CommentDto>> GetAllAsync(int footballMatchId, CancellationToken cancellationToken = default);
    Task<CommentDto> GetByIdAsync(int footballMatchId, int commentId, CancellationToken cancellationToken = default);
    Task<int> Add(int footballMatchId, AddCommentDto dto);
    Task Update(int footballMatchId, int commentId, UpdateCommentDto dto);
    Task Delete(int footballMatchId, int commentId);
}
