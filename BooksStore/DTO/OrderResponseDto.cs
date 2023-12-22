using BooksStore.Consumers.Order;
using BooksStoreEntities.Entities;

namespace BooksStore.DTO;

public class OrderResponseDto
{
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderBookModel> Books { get; set; }
}