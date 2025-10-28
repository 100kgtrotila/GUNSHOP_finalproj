using Application.Weapons.Interfaces;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WeaponRepository : IWeaponRepository
{
    private readonly ApplicationDbContext _context;

    public WeaponRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Weapon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<Weapon> AddAsync(Weapon weapon, CancellationToken cancellationToken = default)
    {
        await _context.Weapons.AddAsync(weapon, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return weapon;
    }

    public async Task UpdateAsync(Weapon weapon, CancellationToken cancellationToken = default)
    {
        _context.Weapons.Update(weapon);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var weapon = await GetByIdAsync(id, cancellationToken);
        if (weapon != null)
        {
            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Weapons
            .AnyAsync(w => w.Id == id, cancellationToken);
    }
}