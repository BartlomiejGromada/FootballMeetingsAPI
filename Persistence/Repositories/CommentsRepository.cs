using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly FootballMeetingsDbContext _dbContext;

    public CommentsRepository(FootballMeetingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Comment>> GetAllComments(int footballMatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Comments
                .Include(c => c.User)
                .ThenInclude(u => u.Role)
                .Where(c => c.FootballMatchId == footballMatchId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }

    public async Task<Comment> GetByIdAsync(int commentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .ThenInclude(u => u.Role)
            .Where(c => c.Id == commentId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Add(Comment comment)
    {
        await _dbContext.Comments
            .AddAsync(comment);
    }

    public async Task<bool> IsCommentBelongsToUser(int commmentId, int userId)
    {
        return await _dbContext.Comments
            .Include(c => c.User)
            .Select(c => new
            {
                c.Id,
                c.UserId
            })
            .AnyAsync(item => item.Id == commmentId && item.UserId == userId);
    }

    public async Task Update(int commentId, Comment comment)
    {
        var commentToUpdate = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);

        commentToUpdate.Content = comment.Content;
    }

    public async Task Delete(int commentId)
    {
        var comment = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);

        _dbContext.Comments
            .Remove(comment);
    }
}
