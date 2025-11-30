namespace Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid WeaponId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Order(
        Guid id,
        string orderNumber,
        Guid customerId,
        Guid weaponId,
        DateTime orderDate,
        OrderStatus status,
        decimal totalAmount,
        DateTime createdAt)
    {
        Id = id;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        WeaponId = weaponId;
        OrderDate = orderDate;
        Status = status;
        TotalAmount = totalAmount;
        CreatedAt = createdAt;
    }

    public static Order New(
        string orderNumber,
        Guid customerId,
        Guid weaponId,
        decimal totalAmount,
        DateTime orderDate)
    {
        return new Order(
            Guid.NewGuid(),
            orderNumber,
            customerId,
            weaponId,
            orderDate,
            OrderStatus.Pending,
            totalAmount,
            DateTime.UtcNow
        );
    }

    public void UpdateStatus(OrderStatus newStatus, string? notes = null)
    {
        Status = newStatus;
        if (notes != null)
        {
            Notes = notes;
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string completionNotes)
    {
        Status = OrderStatus.Completed;
        Notes = completionNotes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
}