using Domain.Orders;

namespace Api.DTOs;

public record CreateOrderRequest(
    string OrderNumber,
    Guid CustomerId,
    Guid WeaponId,
    decimal TotalAmount,
    DateTime OrderDate
);

public record UpdateOrderStatusRequest(
    OrderStatus Status,
    string? Notes
);

public record CompleteOrderRequest(
    string CompletionNotes
);

public record OrderResponse(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    Guid WeaponId,
    DateTime OrderDate,
    OrderStatus Status,
    decimal TotalAmount,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);