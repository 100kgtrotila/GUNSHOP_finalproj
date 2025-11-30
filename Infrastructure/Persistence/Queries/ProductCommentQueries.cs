using Application.Comments.Queries;
using Domain.ProductComment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class ProductCommentQueries : IProductCommentQueries
{
    private readonly ApplicationDbContext _context;
    public ProductCommentQueries(ApplicationDbContext context) => _context = context;

    public async Task<List<ProductComment>> GetByWeaponIdAsync(Guid weaponId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductComments
            .AsNoTracking()
            .Where(c => c.WeaponId == weaponId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}