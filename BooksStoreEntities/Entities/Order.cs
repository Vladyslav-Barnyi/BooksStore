using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class Order : BaseEntity
{
    public required Guid UserId { get; set; }
    public required ApplicationUser User { get; set; }
    [Column(TypeName = "decimal(16,2)")]
    public required decimal TotalAmount { get; set; }
    
    public required OrderStatus Status { get; set; }
    public required DateTime OrderDate { get; set; }
    
    public List<CartItem> CartItems { get; set; } 
}

public enum OrderStatus
{
    Created,
    Paid, 
    Cancelled,
    Holding,
    Processed,
    Failed
}