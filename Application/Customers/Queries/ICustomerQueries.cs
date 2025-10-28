using Domain.Customers;

namespace Application.Customers.Queries;

public interface ICustomerQueries
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<List<Customer>> GetVerifiedCustomersAsync(CancellationToken cancellationToken);
}