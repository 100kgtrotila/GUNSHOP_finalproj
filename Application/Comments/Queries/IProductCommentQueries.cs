using Domain.ProductComment;

namespace Application.Comments.Queries;

public interface IProductCommentQueries
{
    Task<List<ProductComment>> GetByWeaponIdAsync(Guid weaponId, CancellationToken cancellationToken = default);
}