using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class CartItem : BaseEntity
{
    public required Guid ShoppingCartId { get; set; }
    public required ShoppingCart ShoppingCart { get; set; } = null!;
    
    public required Guid BookId { get; set; }
    public required Book Book { get; set; } = null!;
    
    [Column(TypeName = "decimal(6,2)")]
    public required decimal BookPrice { get; set; }
    
    public required int Quantity { get; set; }
}