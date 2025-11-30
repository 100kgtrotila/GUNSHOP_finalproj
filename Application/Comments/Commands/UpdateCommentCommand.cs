using Application.Comments.Interfaces;
using MediatR;

namespace Application.Comments.Commands;

public record UpdateCommentCommand(Guid CommentId, string NewContent) : IRequest;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly IProductCommentRepository _commentRepository;
    public UpdateCommentCommandHandler(IProductCommentRepository commentRepository) => _commentRepository = commentRepository;

    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
        {
            throw new KeyNotFoundException($"Comment with id {request.CommentId} not found.");
        }
        comment.Update(request.NewContent);
        await _commentRepository.UpdateAsync(comment, cancellationToken);
    }
}