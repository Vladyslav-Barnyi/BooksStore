using BooksStore.Consumers.Base;
using BooksStore.Consumers.ShoppingCart;
using BooksStore.Extensions;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ShoppingCartController(IBookService bookService, IShoppingCartService shoppingCartService,
    IUserService userService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<GetShoppingCartResponse>> GetShoppingCartInfoByUserId(Guid userId, 
        CancellationToken ct)
    {
        var user = await userService.FindAsync(userId, ct);
        if (user is null)
            return NotFound($"{nameof(ApplicationUser)} with Id: {userId} was not Found");
        
        var shoppingCart = await shoppingCartService.GetShoppingCartByUserIdAsync(userId, ct);
        if (shoppingCart is null)
            return NotFound($"{nameof(ShoppingCart)} with userId: {userId} was not Found");

        var response = shoppingCart.ToShoppingCartResponse();

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<CreateEntityResponse>> AddCartItem(AddCartItemRequest r, 
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(r.BookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {r.BookId} was not Found");
        
        var shoppingCart = await shoppingCartService.FindShoppingCartAsync(r.ShoppingCartId, ct);
        if (shoppingCart is null)
            return NotFound($"{nameof(ShoppingCart)} with id: {r.ShoppingCartId} was not Found");
        
        var cartItemModel = r.ToCartItem(book, shoppingCart);
        
        var cartItem = await shoppingCartService.AddCartItemAsync(cartItemModel, ct);
        var response = new CreateEntityResponse{ Id = cartItem.Id};

        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> PatchCartItem(Guid cartItemId, 
        JsonPatchDocument<PatchCartItemRequest> r,
        CancellationToken ct)
    {
        var cartItem = await shoppingCartService.FindCartItemAsync(cartItemId, ct);
        if (cartItem is null)
            return NotFound($"{nameof(CartItem)} with id: {cartItemId} was not Found");

        var book = await bookService.FindAsync(cartItem.BookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {cartItem.BookId} was not Found");
        
        var patch = cartItem.ToPatchCartItemRequest();
        r.ApplyTo(patch, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (patch.BookId is not null)
            if (cartItem.BookId != patch.BookId)
                return BadRequest("Provide correct BookId to update book price");
        

        patch.Adapt(cartItem, book.Price);
        await shoppingCartService.UpdateCartItemAsync(cartItem,ct);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCartItem(Guid cartItemId  ,
        CancellationToken ct)
    {
        var cartItem = await shoppingCartService.FindCartItemAsync(cartItemId, ct);
        if (cartItem is null)
            return NotFound($"{nameof(CartItem)} with id: {cartItemId} was not Found");

        await shoppingCartService.DeleteCartItemAsync(cartItem, ct);

        return Ok();
    }
    
}