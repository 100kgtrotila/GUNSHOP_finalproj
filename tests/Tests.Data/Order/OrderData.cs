using Domain.Orders;

namespace Tests.Data.Order;

public static class OrderData
{
    public static Domain.Orders.Order FirstOrder(Guid customerId, Guid weaponId, decimal weaponPrice) => 
        Domain.Orders.Order.New(
            $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}", 
            customerId, 
            weaponId, 
            weaponPrice * 2, 
            DateTime.UtcNow);

    public static Domain.Orders.Order SecondOrder(Guid customerId, Guid weaponId, decimal weaponPrice) => 
        Domain.Orders.Order.New(
            $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}", 
            customerId, 
            weaponId, 
            weaponPrice * 1, 
            DateTime.UtcNow);
    
    public static Domain.Orders.Order ThirdOrder(Guid customerId, Guid weaponId, decimal weaponPrice) => 
        Domain.Orders.Order.New(
            $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}", 
            customerId, 
            weaponId, 
            weaponPrice * 3, 
            DateTime.UtcNow);

    public static Domain.Orders.Order CompletedOrder(Guid customerId, Guid weaponId, decimal weaponPrice)
    {
        var order = Domain.Orders.Order.New(
            $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}", 
            customerId, 
            weaponId, 
            weaponPrice, 
            DateTime.UtcNow);
        order.UpdateStatus(OrderStatus.Completed);
        return order;
    }
}