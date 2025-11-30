using Application.Comments.Interfaces;
using Domain.ProductComment;
using Domain.Weapons;
using MediatR;

namespace Application.Comments.Commands;

public record CreateCommentCommand(Guid WeaponId, string Content) : IRequest<Guid>;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IProductCommentRepository _commentRepository;

    public CreateCommentCommandHandler(IProductCommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = ProductComment.Create(request.WeaponId, request.Content);
        await _commentRepository.AddAsync(comment, cancellationToken);
        return comment.Id;
    }
}