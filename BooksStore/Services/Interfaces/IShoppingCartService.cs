using BooksStoreEntities.Entities;

namespace BooksStore.Services.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCart?> FindShoppingCartAsync(Guid shoppingCartId,
        CancellationToken ct = default);

    Task<CartItem?> FindCartItemAsync(Guid cartItemId,
        CancellationToken ct = default);

    Task<ShoppingCart?> GetShoppingCartByUserIdAsync(Guid userId,
        CancellationToken ct = default);

    Task CreateShoppingCartAsync(ShoppingCart shoppingCart,
        CancellationToken ct = default);

    Task<CartItem> AddCartItemAsync(CartItem cartItem,
        CancellationToken ct = default);

    Task UpdateCartItemAsync(CartItem cartItem,
        CancellationToken ct = default);

    Task DeleteCartItemAsync(CartItem cartItem,
        CancellationToken ct = default);
    
    Task AddItemsToOrderAsync(Order order, List<Guid> itemIds, 
        CancellationToken ct = default);
    
    Task RemoveItemsFromOrderAsync(Order order, List<Guid> itemIds, 
        CancellationToken ct = default);

    Task CleanCartAfterOrderCreationAsync(Guid userId,
        CancellationToken ct = default);
}