using System.Net;
using System.Net.Http.Json;
using Domain.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Customer;
using Api.DTOs;
using Tests.Data.Order;
using Tests.Data.Weapon;
using Xunit;

namespace Api.Tests.Integration.Order;

public class OrderControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/orders";
    
    private readonly Domain.Customers.Customer _customer;
    private readonly Domain.Weapons.Weapon _weapon;
    private readonly Domain.Orders.Order _firstOrder;
    private readonly Domain.Orders.Order _secondOrder;

    public OrderControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _customer = CustomerData.FirstCustomer();
        _weapon = WeaponData.FirstWeapon();
        _firstOrder = OrderData.FirstOrder(_customer.Id, _weapon.Id, _weapon.Price);
        _secondOrder = OrderData.SecondOrder(_customer.Id, _weapon.Id, _weapon.Price);
    }

    public async Task InitializeAsync()
    {
        await Context.Customers.AddAsync(_customer);
        await Context.Weapons.AddAsync(_weapon);
        await Context.Orders.AddRangeAsync(_firstOrder, _secondOrder);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Orders.RemoveRange(Context.Orders);
        Context.Weapons.RemoveRange(Context.Weapons);
        Context.Customers.RemoveRange(Context.Customers);
        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetAllOrders()
    {
        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var orders = await response.ToResponseModel<List<OrderResponse>>();
        orders.Should().NotBeNull();
        orders.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetOrderById()
    {
        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstOrder.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var order = await response.ToResponseModel<OrderResponse>();
        order.Should().NotBeNull();
        order!.Id.Should().Be(_firstOrder.Id);
    }

    [Fact]
    public async Task ShouldCreateOrder()
    {
        // Arrange
        var newOrder = OrderData.ThirdOrder(_customer.Id, _weapon.Id, _weapon.Price);
    
        var request = new CreateOrderRequest(
            newOrder.OrderNumber,
            newOrder.CustomerId,
            newOrder.WeaponId,
            newOrder.TotalAmount,
            newOrder.OrderDate
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdOrder = await response.ToResponseModel<OrderResponse>();
    
        createdOrder.Should().NotBeNull();
        createdOrder!.OrderNumber.Should().Be(request.OrderNumber);
        createdOrder.CustomerId.Should().Be(request.CustomerId);
        createdOrder.WeaponId.Should().Be(request.WeaponId);
        createdOrder.TotalAmount.Should().Be(request.TotalAmount);
        createdOrder.Status.Should().Be(OrderStatus.Pending);
    }



    [Fact]
    public async Task ShouldUpdateOrderStatus()
    {
        // Arrange
        var request = new UpdateOrderStatusRequest(
            OrderStatus.Processing,
            "Order is being processed"
        );

        // Act
        var response = await Client.PatchAsync(
            $"{BaseRoute}/{_firstOrder.Id}/status",
            JsonContent.Create(request));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedOrder = await Context.Orders
            .AsNoTracking()
            .FirstAsync(o => o.Id == _firstOrder.Id);
    
        updatedOrder.Status.Should().Be(OrderStatus.Processing);
        updatedOrder.Notes.Should().Be("Order is being processed");
    }

    [Fact]
    public async Task ShouldCompleteOrder()
    {
        // Arrange
        var request = new CompleteOrderRequest("Order completed successfully");

        // Act
        var response = await Client.PostAsync(
            $"{BaseRoute}/{_firstOrder.Id}/complete",
            JsonContent.Create(request));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var completedOrder = await Context.Orders
            .AsNoTracking()
            .FirstAsync(o => o.Id == _firstOrder.Id);
    
        completedOrder.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteOrder()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_secondOrder.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var deletedOrder = await Context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == _secondOrder.Id);
    
        deletedOrder.Should().BeNull();
    }

}
