using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync(CancellationToken ct = default);

    Task<ShoppingCart> GetShoppingCartByUser(ApplicationUser user, CancellationToken ct = default);

    Task<Order?> FindAsync(Guid orderId, CancellationToken ct = default);

    Task<Order> AddAsync(Order order, CancellationToken ct = default);

    Task UpdateAsync(Order order, CancellationToken ct = default);

    Task RemoveAsync(Order order, CancellationToken ct = default);
}