using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStoreEntities.Entities;

public class ApplicationUser : BaseEntity
{
    [Column(TypeName = "NVARCHAR(222)")]
    public required string UserName { get; set; }
    
    [Column(TypeName = "NVARCHAR(333)")]
    public required string Email { get; set; }
    
    [Column(TypeName = "NVARCHAR(MAX)")]
    public required string Password { get; set; }
    
    public Guid? ShoppingCartId { get; set; }
    public ShoppingCart? ShoppingCart { get; set; } 

    public List<Order> Orders { get; set; } = [];
}