using Domain.Orders;

namespace Application.Orders.Queries;

public interface IOrderQueries
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
    Task<List<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken);
}