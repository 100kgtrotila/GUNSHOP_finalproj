using Application.Weapons.Interfaces;
using Application.Weapons.Queries;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Commands;

public record UpdateWeaponCommand : IRequest<Weapon>
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Manufacturer { get; init; }
    public required string Model { get; init; }
    public required string Caliber { get; init; }
    public required decimal Price { get; init; }
}

public class UpdateWeaponCommandHandler(IWeaponRepository weaponRepository, IWeaponQueries weaponQueries)
    : IRequestHandler<UpdateWeaponCommand, Weapon>
{
    public async Task<Weapon> Handle(UpdateWeaponCommand request, CancellationToken cancellationToken)
    {
        var weapon = await weaponQueries.GetByIdAsync(request.Id, cancellationToken);
        if (weapon is null)
        {
            throw new KeyNotFoundException("Weapon not found.");
        }

        weapon.UpdateDetails(request.Name, request.Manufacturer, request.Model, request.Caliber, request.Price);

        await weaponRepository.UpdateAsync(weapon, cancellationToken);
        return weapon;
    }
}