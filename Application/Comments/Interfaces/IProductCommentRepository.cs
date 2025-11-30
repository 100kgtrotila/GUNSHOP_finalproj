using Domain.ProductComment;
using Domain.Weapons;

namespace Application.Comments.Interfaces;

public interface IProductCommentRepository
{
    Task<ProductComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductComment> AddAsync(ProductComment comment, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductComment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}