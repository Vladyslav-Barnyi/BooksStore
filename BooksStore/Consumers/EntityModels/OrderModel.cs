using BooksStoreEntities.Entities;

namespace BooksStore.Consumers.EntityModels;

public class OrderModel
{
    public required Guid UserId { get; set; }
    public required decimal TotalAmount { get; set; }
    
    public required DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    
    public List<CartItemModel> Items { get; set; } = [];
}