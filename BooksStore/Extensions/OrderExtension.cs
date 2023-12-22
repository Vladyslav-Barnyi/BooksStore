using BooksStore.Consumers.EntityModels;
using BooksStore.Consumers.Order;
using BooksStore.DTO;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class OrderExtension
{
    public static OrderModel ToOrderModel(this Order order)
    {
        var response = new OrderModel
        {
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Items = order.CartItems.Select(ci => ci.ToCartItemModel()).ToList()
        };

        return response;
    }
    
    public static Order ToOrder(this OrderDetailsDto r)
    {
        var response = new Order
        {
            User = r.User,
            UserId = r.UserId,
            OrderDate = r.OrderDate,
            TotalAmount = r.TotalAmount,
            Status = r.Status,
            CartItems = r.CartItems,
        };

        return response;
    }

    public static GetOrderResponse ToOrderResponse(this OrderResponseDto dto)
    {
        var response = new GetOrderResponse
        {
            UserId = dto.UserId,
            OrderDate = dto.OrderDate,
            TotalAmount = dto.TotalAmount,
            Status = dto.Status,
            Books = dto.Books
        };

        return response;
    }
}