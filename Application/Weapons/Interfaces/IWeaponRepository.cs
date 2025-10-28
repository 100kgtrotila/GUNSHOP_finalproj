using Domain.Weapons;

namespace Application.Weapons.Interfaces;

public interface IWeaponRepository
{
    Task<Weapon> AddAsync(Weapon weapon, CancellationToken cancellationToken);
    Task UpdateAsync(Weapon weapon, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}