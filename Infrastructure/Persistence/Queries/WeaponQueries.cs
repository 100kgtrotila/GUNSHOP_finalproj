using Application.Weapons.Interfaces;
using Application.Weapons.Queries;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class WeaponQueries : IWeaponQueries
{
    private readonly ApplicationDbContext _context;

    public WeaponQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Weapon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<List<Weapon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Weapon>> GetByCategoryAsync(WeaponCategory category, CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .AsNoTracking()
            .Where(w => w.Category == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Weapon>> GetByStatusAsync(WeaponStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .AsNoTracking()
            .Where(w => w.Status == status)
            .ToListAsync(cancellationToken);
    }
}