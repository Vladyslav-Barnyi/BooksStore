using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> FindShoppingCartAsync(Guid shoppingCartId,
        CancellationToken ct = default);

    Task<CartItem?> FindCartItemAsync(Guid cartItemId,
        CancellationToken ct = default);

    Task<ShoppingCart?> GetShoppingCartByUserIdAsync(Guid userId,
        CancellationToken ct = default);

    Task CreateShoppingCartAsync(ShoppingCart shoppingCart, CancellationToken ct = default);

    Task<CartItem> AddCartItemAsync(CartItem cartItem,
        CancellationToken ct = default);

    Task AssignScToUser(ApplicationUser user, Guid shoppingCartId,
        CancellationToken ct = default);

    Task UpdateCartItemAsync(CartItem cartItem,
        CancellationToken ct = default);

    Task UpdateShoppingCartAsync(ShoppingCart shoppingCart, 
        CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}