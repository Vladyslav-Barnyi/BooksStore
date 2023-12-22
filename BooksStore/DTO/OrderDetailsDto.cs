using BooksStoreEntities.Entities;

namespace BooksStore.DTO;

public class OrderDetailsDto
{
    public required ApplicationUser User { get; set; }
    public required Guid UserId { get; set; }
    public required decimal TotalAmount { get; set; }
    public required OrderStatus Status { get; set; }
    public required DateTime OrderDate { get; set; }
    public List<CartItem> CartItems { get; set; } 
}