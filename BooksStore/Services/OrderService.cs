using BooksStore.Consumers.EntityModels;
using BooksStore.Consumers.Order;
using BooksStore.DTO;
using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Serilog;

namespace BooksStore.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderResponseDto>> GetOrdersAsync(string? title, Guid? authorId, int? ISBN, FilterType type,
        int page, int pageSize,
        CancellationToken ct = default)
    {
        var orders = await _orderRepository.GetOrdersAsync(ct);

        switch (type)
        {
            case FilterType.Title:
                orders = orders.Where(o => o.CartItems.Any(i => i.Book.Title.Contains(title!))).ToList();
                break;
            case FilterType.Author:
                orders = orders.Where(o => o.CartItems
                    .Any(i => i.Book.Authors.Any(a => a.Id == authorId!.Value))).ToList();
                break;
            case FilterType.ISBN:
                orders = orders.Where(o => o.CartItems
                    .Any(i => i.Book.ISBN == ISBN!.Value)).ToList();
                break;
            case FilterType.All:
                break;
            default:
                throw new ArgumentException("Invalid filter type");
        }

        var paginatedOrders =  orders.Skip((page - 1) * pageSize).Take(pageSize);
        var response = new List<OrderResponseDto>();
        foreach (var order in paginatedOrders)
        {
            var books = new List<OrderBookModel>();
            foreach (var item in order.CartItems)
            {
                var book = new OrderBookModel
                {
                    Title = item.Book.Title,
                    ISBN = item.Book.ISBN,
                    PublicationDate = item.Book.PublicationDate,
                    Price = item.BookPrice,
                    Quantity = item.Quantity,
                    Authors = item.Book.Authors
                        .Select(a => new AuthorModel { FirstName = a.FirstName, LastName = a.LastName }).ToList(),
                    Genres = item.Book.Genres.Select(g => new GenreModel { GenreName = g.Name }).ToList()
                };
                books.Add(book);
            }

            var dto = new OrderResponseDto
            {
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                Books = books
            };
            response.Add(dto);
            return response;
        }

        return response;
    }
    
    public async Task<OrderDetailsDto> GetOrderDetails(ApplicationUser user, CancellationToken ct = default)
    {
        
        var shoppingCart = await _orderRepository.GetShoppingCartByUser(user, ct);

        decimal totalAmount = shoppingCart.CartItems.Sum(item => item.BookPrice * item.Quantity);

        var dto = new OrderDetailsDto
        {
            User = user,
            UserId = user.Id,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Created,
            CartItems = shoppingCart.CartItems,
            TotalAmount = totalAmount
        };

        return dto;
    }

    public async Task<Order?> FindAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await _orderRepository.FindAsync(orderId, ct);

        return order;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        try
        {
            await _orderRepository.AddAsync(order, ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not create order");
            throw;
        }

        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        await _orderRepository.UpdateAsync(order, ct);
    }

    public async Task RemoveAsync(Order order, CancellationToken ct = default)
    {
        await _orderRepository.RemoveAsync(order, ct);
    }
}