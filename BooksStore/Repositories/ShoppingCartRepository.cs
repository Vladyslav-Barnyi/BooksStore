using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ShoppingCartRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShoppingCart?> FindShoppingCartAsync(Guid shoppingCartId, CancellationToken ct = default)
    {
        var shoppingCart = await _dbContext.ShoppingCarts
            .FirstOrDefaultAsync(sc => sc.Id == shoppingCartId, ct);

        return shoppingCart;
    }

    public async Task<CartItem?> FindCartItemAsync(Guid cartItemId, CancellationToken ct = default)
    {
        var cartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId, ct);

        return cartItem;
    }

    public async Task<ShoppingCart?> GetShoppingCartByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var shoppingCart = await _dbContext.ShoppingCarts
            .Include(sc => sc.CartItems)
            .FirstOrDefaultAsync(sc => sc.UserId == userId, ct);

        return shoppingCart;
    }

    public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart, CancellationToken ct = default)
    {
        await _dbContext.AddAsync(shoppingCart, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<CartItem> AddCartItemAsync(CartItem cartItem, CancellationToken ct = default)
    {
        await _dbContext.CartItems.AddAsync(cartItem, ct);
        await SaveChangesAsync(ct);

        return cartItem;
    }

    public async Task AssignScToUser(ApplicationUser user, Guid shoppingCartId, CancellationToken ct = default)
    {
        user.ShoppingCartId = shoppingCartId;
        _dbContext.Update(user);
        await SaveChangesAsync(ct);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem, CancellationToken ct = default)
    {
        _dbContext.Update(cartItem);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateShoppingCartAsync(ShoppingCart shoppingCart, CancellationToken ct = default)
    {
        _dbContext.ShoppingCarts.Update(shoppingCart);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}