using Application.Orders.Interfaces;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Queries;

public record GetAllOrdersQuery : IRequest<List<Order>>;

public class GetAllOrdersQueryHandler(IOrderQueries orderQueries)
    : IRequestHandler<GetAllOrdersQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await orderQueries.GetAllAsync(cancellationToken);
    }
}