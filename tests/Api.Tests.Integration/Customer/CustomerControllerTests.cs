using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Api.DTOs;
using Tests.Data.Customer;
using Xunit;

namespace Api.Tests.Integration.Customer;

public class CustomerControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/customers";
    
    private readonly Domain.Customers.Customer _firstCustomer;
    private readonly Domain.Customers.Customer _secondCustomer;

    public CustomerControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _firstCustomer = CustomerData.FirstCustomer();
        _secondCustomer = CustomerData.SecondCustomer();
    }

    public async Task InitializeAsync()
    {
        await Context.Customers.AddRangeAsync(_firstCustomer, _secondCustomer);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Customers.RemoveRange(Context.Customers);
        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetAllCustomers()
    {
        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var customers = await response.ToResponseModel<List<CustomerResponse>>();
        customers.Should().NotBeNull();
        customers.Should().HaveCount(2);
        customers.Should().Contain(c => c.Email == _firstCustomer.Email);
        customers.Should().Contain(c => c.Email == _secondCustomer.Email);
    }

    [Fact]
    public async Task ShouldGetCustomerById()
    {
        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstCustomer.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var customer = await response.ToResponseModel<CustomerResponse>();
        customer.Should().NotBeNull();
        customer!.Id.Should().Be(_firstCustomer.Id);
        customer.Email.Should().Be(_firstCustomer.Email);
        customer.FirstName.Should().Be(_firstCustomer.FirstName);
        customer.LastName.Should().Be(_firstCustomer.LastName);
    }

    [Fact]
    public async Task ShouldCreateCustomer()
    {
        // Arrange
        var newCustomer = CustomerData.ThirdCustomer();
        var request = new CreateCustomerRequest(
            newCustomer.FirstName,
            newCustomer.LastName,
            newCustomer.Email,
            newCustomer.PhoneNumber,
            newCustomer.LicenseNumber
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    
        var createdCustomer = await response.ToResponseModel<CustomerResponse>();
        createdCustomer.Should().NotBeNull();
        createdCustomer!.Email.Should().Be(request.Email);
        createdCustomer.FirstName.Should().Be(request.FirstName);
        createdCustomer.LastName.Should().Be(request.LastName);
        createdCustomer.PhoneNumber.Should().Be(request.PhoneNumber);
        createdCustomer.LicenseNumber.Should().Be(request.LicenseNumber);
        createdCustomer.IsVerified.Should().BeFalse();

        var dbCustomer = await Context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == createdCustomer.Id);
    
        dbCustomer.Should().NotBeNull();
        dbCustomer!.Email.Should().Be(request.Email);
        dbCustomer.FirstName.Should().Be(request.FirstName);
        dbCustomer.LastName.Should().Be(request.LastName);
    }

    
    [Fact]
    public async Task ShouldUpdateCustomer()
    {
        // Arrange
        var request = new UpdateCustomerRequest(
            "John Updated",
            "Doe Updated",
            "john.updated@test.com",
            "+380509999999"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstCustomer.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedCustomer = await Context.Customers
            .AsNoTracking()
            .FirstAsync(c => c.Id == _firstCustomer.Id);
    
        updatedCustomer.FirstName.Should().Be("John Updated");
        updatedCustomer.LastName.Should().Be("Doe Updated");
        updatedCustomer.Email.Should().Be("john.updated@test.com");
        updatedCustomer.PhoneNumber.Should().Be("+380509999999");
    }

    [Fact]
    public async Task ShouldVerifyCustomer()
    {
        // Arrange
        _firstCustomer.IsVerified.Should().BeFalse();

        // Act
        var response = await Client.PostAsync($"{BaseRoute}/{_firstCustomer.Id}/verify", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var verifiedCustomer = await Context.Customers
            .AsNoTracking()
            .FirstAsync(c => c.Id == _firstCustomer.Id);
        
        verifiedCustomer.IsVerified.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldDeleteCustomer()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstCustomer.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var deletedCustomer = await Context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == _firstCustomer.Id);
    
        deletedCustomer.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
