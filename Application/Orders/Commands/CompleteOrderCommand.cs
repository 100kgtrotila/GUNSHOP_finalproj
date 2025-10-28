using Application.Orders.Interfaces;
using Application.Orders.Queries;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands;

public record CompleteOrderCommand : IRequest<Order>
{
    public Guid Id { get; set; }
    public required string CompletionNotes { get; init; }
}

public class CompleteOrderCommandHandler(IOrderRepository orderRepository, IOrderQueries orderQueries)
    : IRequestHandler<CompleteOrderCommand, Order>
{
    public async Task<Order> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderQueries.GetByIdAsync(request.Id, cancellationToken);
        if (order is null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.Complete(request.CompletionNotes);

        await orderRepository.UpdateAsync(order, cancellationToken);
        return order;
    }
}