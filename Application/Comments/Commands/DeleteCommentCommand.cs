using Application.Comments.Interfaces;
using MediatR;

namespace Application.Comments.Commands;

public record DeleteCommentCommand(Guid CommentId) : IRequest;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IProductCommentRepository _repository;
    public DeleteCommentCommandHandler(IProductCommentRepository repository) => _repository = repository;

    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.CommentId, cancellationToken);
    }
}