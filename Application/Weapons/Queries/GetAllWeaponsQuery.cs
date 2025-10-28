using Application.Weapons.Interfaces;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Queries;

public record GetAllWeaponsQuery : IRequest<List<Weapon>>;

public class GetAllWeaponsQueryHandler(IWeaponQueries weaponQueries)
    : IRequestHandler<GetAllWeaponsQuery, List<Weapon>>
{
    public async Task<List<Weapon>> Handle(GetAllWeaponsQuery request, CancellationToken cancellationToken)
    {
        return await weaponQueries.GetAllAsync(cancellationToken);
    }
}