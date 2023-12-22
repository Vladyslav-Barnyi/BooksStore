namespace BooksStoreEntities.Entities;

public class ShoppingCart : BaseEntity
{
    public required Guid UserId{ get; set; }
    public required ApplicationUser ApplicationUser { get; set; }
    
    public List<CartItem> CartItems { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}