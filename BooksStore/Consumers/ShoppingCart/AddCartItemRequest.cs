using Microsoft.Build.Framework;

namespace BooksStore.Consumers.ShoppingCart;

public class AddCartItemRequest
{
    [Required] public Guid ShoppingCartId { get; set; }
    [Required] public Guid BookId { get; set; }
    [Required] public int Quantity { get; set; }
}