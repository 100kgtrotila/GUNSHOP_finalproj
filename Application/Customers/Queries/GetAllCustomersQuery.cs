using Application.Customers.Interfaces;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public record GetAllCustomersQuery : IRequest<List<Customer>>;

public class GetAllCustomersQueryHandler(ICustomerQueries customerQueries)
    : IRequestHandler<GetAllCustomersQuery, List<Customer>>
{
    public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await customerQueries.GetAllAsync(cancellationToken);
    }
}