using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetOrdersAsync(CancellationToken ct = default)
    {
        var orders = await _dbContext.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Book)
            .ThenInclude(b => b.Authors)
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Book)
            .ThenInclude(b => b.Genres)
            .ToListAsync(ct);

        return orders;
    }

    public async Task<ShoppingCart> GetShoppingCartByUser(ApplicationUser user, CancellationToken ct = default)
    {
        var shoppingCart = await _dbContext.ShoppingCarts.Include(sc => sc.CartItems)
            .FirstOrDefaultAsync(sc => sc.Id == user.ShoppingCartId, ct);

        return shoppingCart!;
    }

    public async Task<Order?> FindAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await _dbContext.Orders.Include(sc => sc.CartItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        return order;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        await _dbContext.Orders.AddAsync(order, ct);
        await _dbContext.SaveChangesAsync(ct);

        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _dbContext.Update(order);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Order order, CancellationToken ct = default)
    {
        _dbContext.Remove(order);
        await _dbContext.SaveChangesAsync(ct);
    }
}