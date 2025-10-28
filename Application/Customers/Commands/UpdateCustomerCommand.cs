using Application.Customers.Interfaces;
using Application.Customers.Queries;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Commands;

public record UpdateCustomerCommand : IRequest<Customer>
{
    public Guid Id { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, ICustomerQueries customerQueries)
    : IRequestHandler<UpdateCustomerCommand, Customer>
{
    public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerQueries.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }

        customer.UpdateDetails(request.FirstName, request.LastName, request.Email, request.PhoneNumber);

        await customerRepository.UpdateAsync(customer, cancellationToken);
        return customer;
    }
}