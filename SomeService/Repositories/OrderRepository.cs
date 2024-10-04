using SomeService.Models;

namespace SomeService.Repositories;

public class OrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(int cost)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Cost = cost
        };
        
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        var orderForRemove = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        if (orderForRemove == null)
        {
            return;
        }
        _context.Remove(orderForRemove);
        await _context.SaveChangesAsync();
    }
}