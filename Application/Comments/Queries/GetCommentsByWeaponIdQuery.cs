using Domain.Weapons;
using Application.Comments.Interfaces;
using Domain.ProductComment;
using MediatR;

namespace Application.Comments.Queries;

public record GetCommentsByWeaponIdQuery(Guid WeaponId) : IRequest<List<ProductComment>>;

public class GetCommentsByWeaponIdQueryHandler : IRequestHandler<GetCommentsByWeaponIdQuery, List<ProductComment>>
{
    private readonly IProductCommentQueries _commentQueries;
    public GetCommentsByWeaponIdQueryHandler(IProductCommentQueries commentQueries) => _commentQueries = commentQueries;

    public async Task<List<ProductComment>> Handle(GetCommentsByWeaponIdQuery request, CancellationToken cancellationToken)
    {
        return await _commentQueries.GetByWeaponIdAsync(request.WeaponId, cancellationToken);
    }
}