using Application.Comments.Interfaces;
using Domain.Weapons;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.ProductComment;

namespace Infrastructure.Persistence.Repositories;

public class ProductCommentRepository : IProductCommentRepository
{
    private readonly ApplicationDbContext _context;

    public ProductCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductComments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<ProductComment> AddAsync(ProductComment comment, CancellationToken cancellationToken = default)
    {
        await _context.ProductComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return comment;
    }

    public async Task UpdateAsync(ProductComment comment, CancellationToken cancellationToken = default)
    {
        _context.ProductComments.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.ProductComments.FindAsync(new object[] { id }, cancellationToken);
        if (comment != null)
        {
            _context.ProductComments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}