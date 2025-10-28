using Application.Weapons.Interfaces;
using Application.Weapons.Queries;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Commands;

public record ChangeWeaponStatusCommand : IRequest<Weapon>
{
    public Guid Id { get; set; }
    public required WeaponStatus NewStatus { get; init; }
}

public class ChangeWeaponStatusCommandHandler(IWeaponRepository weaponRepository, IWeaponQueries weaponQueries)
    : IRequestHandler<ChangeWeaponStatusCommand, Weapon>
{
    public async Task<Weapon> Handle(ChangeWeaponStatusCommand request, CancellationToken cancellationToken)
    {
        var weapon = await weaponQueries.GetByIdAsync(request.Id, cancellationToken);
        if (weapon is null)
        {
            throw new KeyNotFoundException("Weapon not found.");
        }

        weapon.ChangeStatus(request.NewStatus);

        await weaponRepository.UpdateAsync(weapon, cancellationToken);
        return weapon;
    }
}