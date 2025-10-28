using Application.Orders.Interfaces;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands;

public record CreateOrderCommand : IRequest<Order>
{
    public required string OrderNumber { get; init; }
    public required Guid CustomerId { get; init; }
    public required Guid WeaponId { get; init; }
    public required decimal TotalAmount { get; init; }
    public required DateTime OrderDate { get; init; }
}

public class CreateOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.New(
            request.OrderNumber,
            request.CustomerId,
            request.WeaponId,
            request.TotalAmount,
            request.OrderDate
        );

        return await orderRepository.AddAsync(order, cancellationToken);
    }
}