namespace BooksStore.Consumers.ShoppingCart;

public class PatchCartItemRequest
{
    
    public int? Quantity { get; set; }
    public Guid? BookId { get; set; }
    //to update BookPrice correctly user should provide boookId 

}