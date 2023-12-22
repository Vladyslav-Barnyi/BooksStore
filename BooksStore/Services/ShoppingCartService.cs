using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Serilog;

namespace BooksStore.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    public async Task<ShoppingCart?> FindShoppingCartAsync(Guid shoppingCartId,
        CancellationToken ct = default)
    {
        var shoppingCart = await _shoppingCartRepository.FindShoppingCartAsync(shoppingCartId, ct);

        return shoppingCart;
    }

    public async Task<CartItem?> FindCartItemAsync(Guid cartItemId,
        CancellationToken ct = default)
    {
        var cartItem = await _shoppingCartRepository.FindCartItemAsync(cartItemId, ct);

        return cartItem;
    }

    public async Task<ShoppingCart?> GetShoppingCartByUserIdAsync(Guid userId,
        CancellationToken ct = default)
    {
        var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(userId, ct);

        return shoppingCart;
    }

    public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart,
        CancellationToken ct = default)
    {
        await _shoppingCartRepository.CreateShoppingCartAsync(shoppingCart, ct);
        await _shoppingCartRepository.AssignScToUser(shoppingCart.ApplicationUser, shoppingCart.Id, ct);
    }

    public async Task<CartItem> AddCartItemAsync(CartItem cartItem,
        CancellationToken ct = default)
    {
        try
        {
            await _shoppingCartRepository.AddCartItemAsync(cartItem, ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not add cart item");
            throw;
        }

        var shoppingCart = await FindShoppingCartAsync(cartItem.ShoppingCartId, ct);
        shoppingCart?.CartItems.Add(cartItem);

        await _shoppingCartRepository.UpdateShoppingCartAsync(shoppingCart!, ct);

        return cartItem;
    }

    public async Task UpdateCartItemAsync(CartItem cartItem,
        CancellationToken ct = default)
    {
        await _shoppingCartRepository.UpdateCartItemAsync(cartItem, ct);
    }

    public async Task DeleteCartItemAsync(CartItem cartItem,
        CancellationToken ct = default)
    {
        var shoppingCart = await FindShoppingCartAsync(cartItem.ShoppingCartId, ct);
        shoppingCart?.CartItems.Remove(cartItem);
        await _shoppingCartRepository.SaveChangesAsync(ct);
    }

    public async Task AddItemsToOrderAsync(Order order, List<Guid> itemIds,
        CancellationToken ct = default)
    {
        var itemsToAdd = new List<CartItem>();
        foreach (var itemId in itemIds)
        {
            var item = await _shoppingCartRepository.FindCartItemAsync(itemId, ct);
            if (item == null) continue;

            order.TotalAmount += item.BookPrice * item.Quantity;
            itemsToAdd.Add(item);
        }

        order.CartItems.AddRange(itemsToAdd);
        await _shoppingCartRepository.SaveChangesAsync(ct);
    }

    public async Task RemoveItemsFromOrderAsync(Order order, List<Guid> itemIds,
        CancellationToken ct = default)
    {
        foreach (var itemId in itemIds)
        {
            var item = await _shoppingCartRepository.FindCartItemAsync(itemId, ct);

            if (item == null) continue;

            order.CartItems.Remove(item);
        }

        order.TotalAmount = order.CartItems.Sum(item => item.BookPrice * item.Quantity);
        await _shoppingCartRepository.SaveChangesAsync(ct);
    }

    public async Task CleanCartAfterOrderCreationAsync(Guid userId,
        CancellationToken ct = default)
    {
        var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(userId, ct);

        shoppingCart!.CartItems = new List<CartItem>();
        await _shoppingCartRepository.UpdateShoppingCartAsync(shoppingCart, ct);
    }
}