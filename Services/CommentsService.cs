using AutoMapper;
using Contracts.Models.Comments;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public class CommentsService : ICommentsService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CommentsService(IRepositoryManager repositoryManager, IMapper mapper,
        IUserContextService userContextService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<List<CommentDto>> GetAllAsync(int footballMatchId, CancellationToken cancellationToken = default)
    {
        await FootballMatchExists(footballMatchId, cancellationToken);

        var comments = await _repositoryManager.CommentsRepository.GetAllComments(footballMatchId, cancellationToken);

        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<CommentDto> GetByIdAsync(int footballMatchId, int commentId, CancellationToken cancellationToken = default)
    {
        await FootballMatchExists(footballMatchId, cancellationToken);

        var comment = await CommentExists(commentId, footballMatchId, cancellationToken);

        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<int> Add(int footballMatchId, AddCommentDto dto)
    {
        await FootballMatchExists(footballMatchId);

        await CheckUserPermissions(footballMatchId);

        var comment = _mapper.Map<Comment>(dto);
        comment.FootballMatchId = footballMatchId;
        comment.CreatedAt = DateTime.Now;
        comment.UserId = _userContextService.GetUserId;

        await _repositoryManager.CommentsRepository.Add(comment);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return comment.Id;
    }

    public async Task Update(int footballMatchId, int commentId, UpdateCommentDto dto)
    {
        await FootballMatchExists(footballMatchId);

        await CommentExists(commentId, footballMatchId);

        await CheckUserPermissions(footballMatchId);

        //If role is "User" then check if comments belongs to specific user
        if(_userContextService.GetUserRole == "User")
        {
            var isCommentBelongsToUser = await _repositoryManager.CommentsRepository
                .IsCommentBelongsToUser(commentId, _userContextService.GetUserId);
            if (!isCommentBelongsToUser)
            {
                throw new ForbidException();
            }
        }

        var commentToUpdate = _mapper.Map<Comment>(dto);
        await _repositoryManager.CommentsRepository.Update(commentId, commentToUpdate);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task Delete(int footballMatchId, int commentId)
    {
        await FootballMatchExists(footballMatchId);

        await CommentExists(commentId, footballMatchId);

        await CheckUserPermissions(footballMatchId);

        //If role is "User" then check if comments belongs to specific user
        if (_userContextService.GetUserRole == "User")
        {
            var isCommentBelongsToUser = await _repositoryManager.CommentsRepository
                .IsCommentBelongsToUser(commentId, _userContextService.GetUserId);
            if (!isCommentBelongsToUser)
            {
                throw new ForbidException();
            }
        }

        await _repositoryManager.CommentsRepository.Delete(commentId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }


    #region Private methods
    private async Task FootballMatchExists(int footballMatchId, CancellationToken cancellationToken = default)
    {
        var isFootballMatchExists = await _repositoryManager.FootballMatchesRepository.ExistsAsync(footballMatchId, cancellationToken);
        if (!isFootballMatchExists)
        {
            throw new NotFoundException($"Football match with id: {footballMatchId} cannot be found");
        }
    }

    public async Task<Comment> CommentExists(int commentId, int footballMatchId, CancellationToken cancellationToken = default)
    {
        var comment = await _repositoryManager.CommentsRepository.GetByIdAsync(commentId, cancellationToken);

        if (comment == null || comment.FootballMatchId != footballMatchId)
        {
            throw new NotFoundException($"Comment with id: {commentId} cannot be found");
        }

        return comment;
    }

    private async Task CheckUserPermissions(int footballMatchId)
    {
        var existsPlayerInFootballMatch = await _repositoryManager.FootballMatchesRepository
           .ExistsPlayerInFootballMatchAsync(footballMatchId, _userContextService.GetUserId);

        if (_userContextService.GetUserRole != "Admin" && _userContextService.GetUserRole != "Creator" &&
           !existsPlayerInFootballMatch)
        {
            throw new ForbidException();
        }
    }
    #endregion
}
