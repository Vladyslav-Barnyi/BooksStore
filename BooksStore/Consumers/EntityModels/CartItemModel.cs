namespace BooksStore.Consumers.EntityModels;

public class CartItemModel
{
    public required Guid ItemId { get; set; }
    public required Guid BookId { get; set; }
    public required Guid ShoppingCartId { get; set; }
    
    public required decimal BookPrice { get; set; }
    public required int Quantity { get; set; }
}