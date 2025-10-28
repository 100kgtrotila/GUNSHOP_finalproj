using Domain.Weapons;

namespace Application.Weapons.Queries;

public interface IWeaponQueries
{
    Task<Weapon?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Weapon>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Weapon>> GetByCategoryAsync(WeaponCategory category, CancellationToken cancellationToken);
    Task<List<Weapon>> GetByStatusAsync(WeaponStatus status, CancellationToken cancellationToken);
}