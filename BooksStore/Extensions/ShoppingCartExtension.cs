using BooksStore.Consumers.EntityModels;
using BooksStore.Consumers.ShoppingCart;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class ShoppingCartExtension
{
    public static CartItem ToCartItem(this AddCartItemRequest r, Book book, ShoppingCart shoppingCart)
    {
        var response = new CartItem
        {
            BookId = r.BookId,
            ShoppingCartId = r.ShoppingCartId,
            Quantity = r.Quantity,
            BookPrice = book.Price,
            Book = book,
            ShoppingCart = shoppingCart
        };

        return response;
    }

    public static PatchCartItemRequest ToPatchCartItemRequest(this CartItem cartItem)
    {
        var response = new PatchCartItemRequest
        {
            BookId = cartItem.BookId,
            Quantity = cartItem.Quantity
        };

        return response;
    }

    public static GetShoppingCartResponse ToShoppingCartResponse(this ShoppingCart shoppingCart)
    {
        var response = new GetShoppingCartResponse
        {
            Id = shoppingCart.Id,
            CartItemModels = shoppingCart.CartItems.Select(ci => ci.ToCartItemModel()).ToList()
        };

        return response;
    }

    public static CartItemModel ToCartItemModel(this CartItem cartItem)
    {
        var response = new CartItemModel
        {
            ItemId = cartItem.Id,
            BookId = cartItem.BookId,
            BookPrice = cartItem.BookPrice,
            Quantity = cartItem.Quantity,
            ShoppingCartId = cartItem.ShoppingCartId
        };

        return response;
    }
    
    public static void Adapt(
        this PatchCartItemRequest r, CartItem cartItem, decimal bookPrice)
    {
        if (r.Quantity is not null)
            cartItem.Quantity = (int)r.Quantity;
        
        if (r.BookId is not null)
            cartItem.BookPrice = bookPrice;
    }
}