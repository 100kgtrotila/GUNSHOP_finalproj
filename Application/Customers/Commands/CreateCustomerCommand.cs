using Application.Customers.Interfaces;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Commands;

public record CreateCustomerCommand : IRequest<Customer>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string LicenseNumber { get; init; }
}

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand, Customer>
{
    public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.New(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.LicenseNumber
        );

        return await customerRepository.AddAsync(customer, cancellationToken);
    }
}