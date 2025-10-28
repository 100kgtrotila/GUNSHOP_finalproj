using Application.Customers.Interfaces;
using Application.Customers.Queries;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Commands;

public record VerifyCustomerCommand(Guid Id) : IRequest<Customer>;

public class VerifyCustomerCommandHandler(ICustomerRepository customerRepository, ICustomerQueries customerQueries)
    : IRequestHandler<VerifyCustomerCommand, Customer>
{
    public async Task<Customer> Handle(VerifyCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerQueries.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }

        customer.Verify();

        await customerRepository.UpdateAsync(customer, cancellationToken);
        return customer;
    }
}