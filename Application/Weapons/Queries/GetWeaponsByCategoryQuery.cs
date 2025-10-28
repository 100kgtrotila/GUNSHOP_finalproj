using Application.Weapons.Interfaces;
using Domain.Weapons;
using MediatR;

namespace Application.Weapons.Queries;

public record GetWeaponsByCategoryQuery(WeaponCategory Category) : IRequest<List<Weapon>>;

public class GetWeaponsByCategoryQueryHandler(IWeaponQueries weaponQueries)
    : IRequestHandler<GetWeaponsByCategoryQuery, List<Weapon>>
{
    public async Task<List<Weapon>> Handle(GetWeaponsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await weaponQueries.GetByCategoryAsync(request.Category, cancellationToken);
    }
}