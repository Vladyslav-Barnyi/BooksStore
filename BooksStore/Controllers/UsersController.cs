using BooksStore.Consumers.Base;
using BooksStore.Consumers.User;
using BooksStore.Extensions;
using BooksStore.Filters;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController(IUserService userService, IShoppingCartService shoppingCartService) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<List<GetUserResponse>>> GetUsers([FromQuery] PaginationFilter filter,
        CancellationToken ct)
    {
        var users = await userService.GetUsersAsync(filter.Page, filter.PageSize, ct);

        var response = users.Select(u=>u.ToUserResponse());
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetUserResponse>> GetUser(Guid userId, 
        CancellationToken ct)
    {
        var user = await userService.FindAsync(userId, ct);
        if (user is null)
            return NotFound($"{nameof(ApplicationUser)} with Id: {userId} was not Found");

        var response = user.ToUserResponse();
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<CreateEntityResponse>> AddUser(AddUserRequest r,
        CancellationToken ct)
    {
        var userModel = r.ToUser();
        var user = await userService.AddAsync(userModel, ct);

        var shoppingCart = new ShoppingCart {
            UserId = user.Id,
            ApplicationUser = user
        };
        await shoppingCartService.CreateShoppingCartAsync(shoppingCart, ct);
        
        var response = new CreateEntityResponse { Id = user.Id };

        return Ok(response);

    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateUser(Guid userId,
        [FromBody] JsonPatchDocument<PatchUserRequest> r,
        CancellationToken ct)
    {
        var user = await userService.FindAsync(userId, ct);
        if (user is null)
            return NotFound($"{nameof(ApplicationUser)} with Id: {userId} was not Found");
        
        var patch = user.ToPatchUserRequest();
        
        var serviceResult = await userService.CheckModelState(r, patch, ModelState);
        if (!serviceResult)
            return BadRequest(ModelState);

        patch.Adapt(user);
        await userService.UpdateAsync(user,ct);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(Guid userId,
        CancellationToken ct)
    {
        var user = await userService.FindAsync(userId, ct);
        if (user is null)
            return NotFound($"{nameof(ApplicationUser)} with Id: {userId} was not Found");

        await userService.RemoveAsync(user, ct);

        return Ok();
    }
}