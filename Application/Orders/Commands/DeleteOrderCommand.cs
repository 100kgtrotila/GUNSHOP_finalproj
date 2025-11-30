using Application.Orders.Interfaces;
using MediatR;

namespace Application.Orders.Commands;

public record DeleteOrderCommand(Guid OrderId) : IRequest;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _repository;

    public DeleteOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.OrderId, cancellationToken);
    }
}