using Application.Customers.Interfaces;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<Customer?>;

public class GetCustomerByIdQueryHandler(ICustomerQueries customerQueries)
    : IRequestHandler<GetCustomerByIdQuery, Customer?>
{
    public async Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await customerQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}