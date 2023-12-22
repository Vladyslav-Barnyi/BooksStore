using BooksStore.Consumers.EntityModels;

namespace BooksStore.Consumers.ShoppingCart;

public class GetShoppingCartResponse
{
    public required Guid Id { get; set; }
    public List<CartItemModel> CartItemModels { get; set; } = [];
}