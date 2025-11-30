using Application.Weapons.Interfaces;
using MediatR;

namespace Application.Weapons.Commands;

public record DeleteWeaponCommand(Guid WeaponId) : IRequest;

public class DeleteWeaponCommandHandler : IRequestHandler<DeleteWeaponCommand>
{
    private readonly IWeaponRepository _repository;

    public DeleteWeaponCommandHandler(IWeaponRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteWeaponCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.WeaponId, cancellationToken);
    }
}