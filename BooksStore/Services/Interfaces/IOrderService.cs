using BooksStore.Consumers.Order;
using BooksStore.DTO;
using BooksStoreEntities.Entities;

namespace BooksStore.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetOrdersAsync(string? title, Guid? authorId, int? ISBN, FilterType type,
        int page, int pageSize,
        CancellationToken ct = default);
    
    Task<OrderDetailsDto> GetOrderDetails(ApplicationUser user,
        CancellationToken ct = default);

    Task<Order?> FindAsync(Guid orderId,
        CancellationToken ct = default);

    Task<Order> AddAsync(Order order,
        CancellationToken ct = default);

    Task UpdateAsync(Order order,
        CancellationToken ct = default);

    Task RemoveAsync(Order order,
        CancellationToken ct = default);
}