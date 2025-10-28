using Application.Orders.Interfaces;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<Order?>;

public class GetOrderByIdQueryHandler(IOrderQueries orderQueries)
    : IRequestHandler<GetOrderByIdQuery, Order?>
{
    public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await orderQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}