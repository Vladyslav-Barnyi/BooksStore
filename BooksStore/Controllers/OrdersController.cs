using BooksStore.Consumers.Base;
using BooksStore.Consumers.Order;
using BooksStore.Extensions;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrdersController(
    IOrderService orderService,
    IShoppingCartService shoppingCartService,
    IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetOrderResponse>>> GetOrders([FromQuery] GetOrdersRequest r,
        CancellationToken ct)
    {
        if (!r.IsValid())
            return BadRequest("Please provide maximum one filter: Title, AuthorId, or ISBN.");

        var orders = await orderService
            .GetOrdersAsync(r.Title, r.AuthorId, r.ISBN, r.FilterType, r.Page, r.PageSize, ct);

        var response = orders.Select(o => o.ToOrderResponse());

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateEntityResponse>> AddOrder(Guid userId,
        CancellationToken ct)
    {
        var user = await userService.FindAsync(userId, ct);
        if (user is null)
            return NotFound($"{nameof(ApplicationUser)} with Id: {userId} was not Found");

        var orderDetails = await orderService.GetOrderDetails(user, ct);


        var orderModel = orderDetails.ToOrder();

        var order = await orderService.AddAsync(orderModel, ct);
        var response = new CreateEntityResponse { Id = order.Id };
        await shoppingCartService.CleanCartAfterOrderCreationAsync(userId, ct);
        
        return Ok(response);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateOrder(PatchOrderRequest r,
        CancellationToken ct)
    {
        var order = await orderService.FindAsync(r.OrderId, ct);
        if (order is null)
            return NotFound($"{nameof(Order)} with id: {r.OrderId} was not Found");

        if (order.Status != OrderStatus.Created && order.Status != OrderStatus.Holding)
            return Forbid("It's too late");

        if (r.AddItems)
        {
            await shoppingCartService.AddItemsToOrderAsync(order, r.ItemIds, ct);
        }
        else
        {
            await shoppingCartService.RemoveItemsFromOrderAsync(order, r.ItemIds, ct);
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveOrder(Guid orderId,
        CancellationToken ct)
    {
        var order = await orderService.FindAsync(orderId, ct);
        if (order is null)
            return NotFound($"{nameof(Order)} with id: {orderId} was not Found");

        await orderService.RemoveAsync(order, ct);

        return Ok();
    }
}