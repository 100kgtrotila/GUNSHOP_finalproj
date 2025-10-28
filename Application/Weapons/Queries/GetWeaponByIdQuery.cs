using Application.Weapons.Interfaces;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Queries;

public record GetWeaponByIdQuery(Guid Id) : IRequest<Weapon?>;

public class GetWeaponByIdQueryHandler(IWeaponQueries weaponQueries)
    : IRequestHandler<GetWeaponByIdQuery, Weapon?>
{
    public async Task<Weapon?> Handle(GetWeaponByIdQuery request, CancellationToken cancellationToken)
    {
        return await weaponQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}