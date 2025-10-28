using Application.Orders.Interfaces;
using Application.Orders.Queries;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands;

public record UpdateOrderStatusCommand : IRequest<Order>
{
    public Guid Id { get; set; }
    public required OrderStatus NewStatus { get; init; }
    public string? Notes { get; init; }
}

public class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IOrderQueries orderQueries)
    : IRequestHandler<UpdateOrderStatusCommand, Order>
{
    public async Task<Order> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderQueries.GetByIdAsync(request.Id, cancellationToken);
        if (order is null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.UpdateStatus(request.NewStatus, request.Notes);

        await orderRepository.UpdateAsync(order, cancellationToken);
        return order;
    }
}