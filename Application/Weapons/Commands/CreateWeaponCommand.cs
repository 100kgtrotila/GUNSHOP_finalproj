using Application.Weapons.Interfaces;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Commands;

public record CreateWeaponCommand : IRequest<Weapon>
{
    public required string Name { get; init; }
    public required string Manufacturer { get; init; }
    public required string Model { get; init; }
    public required string Caliber { get; init; }
    public required string SerialNumber { get; init; }
    public required decimal Price { get; init; }
    public required WeaponCategory Category { get; init; }
}

public class CreateWeaponCommandHandler(IWeaponRepository weaponRepository)
    : IRequestHandler<CreateWeaponCommand, Weapon>
{
    public async Task<Weapon> Handle(CreateWeaponCommand request, CancellationToken cancellationToken)
    {
        var weapon = Weapon.New(
            request.Name,
            request.Manufacturer,
            request.Model,
            request.Caliber,
            request.SerialNumber,
            request.Price,
            request.Category
        );

        return await weaponRepository.AddAsync(weapon, cancellationToken);
    }
}