using BooksStore.Consumers.Order;
using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly OrderService _orderService;


    public OrderServiceTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderService = new OrderService(_orderRepositoryMock.Object);
    }

    private List<Order> GenerateMockOrders()
    {
        var user = GenerateMockUser();

        var order = new Order
        {
            Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 300, User = user, UserId = Guid.NewGuid()
        };
        var order1 = new Order
        {
            Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 300, User = user, UserId = Guid.NewGuid()
        };
        var order2 = new Order
        {
            Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 300, User = user, UserId = Guid.NewGuid()
        };

        var orders = new List<Order>();
        orders.Add(order);
        orders.Add(order1);
        orders.Add(order2);

        return orders;
    }

    private Order GenerateMockOrder()
    {
        var user = GenerateMockUser();

        var order = new Order
        {
            Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 300, User = user, UserId = Guid.NewGuid()
        };

        return order;
    }

    private ShoppingCart GenerateMockShoppingCart()
    {
        var user = GenerateMockUser();
        var shoppingCart = new ShoppingCart
        {
            Id = Guid.NewGuid(), UserId = Guid.NewGuid(), ApplicationUser = user,
            CartItems = new List<CartItem>()
        };

        return shoppingCart;
    }

    private ApplicationUser GenerateMockUser()
    {
        var user = new ApplicationUser
            { UserName = "Illuminati", Email = "illuminati@gmail.com", Password = "324fdsa" };
        return user;
    }

    [Theory]
    [InlineData(FilterType.Title, "Test Title", null, null)]
    [InlineData(FilterType.Author, null, "12345678-1234-1234-1234-123456789012", null)] 
    [InlineData(FilterType.ISBN, null, null, 123456)]
    [InlineData(FilterType.All, null, null, null)]
    public async Task GetOrdersAsync_ReturnsFilteredAndPaginatedOrders(FilterType filterType, string? title, string? authorIdString, int? ISBN)
    {
        var authorId = authorIdString != null ? Guid.Parse(authorIdString) : (Guid?)null;
        var mockOrders = GenerateMockOrders(); 
        _orderRepositoryMock.Setup(repo => repo.GetOrdersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockOrders);

        var result = await _orderService.GetOrdersAsync(title, authorId, ISBN, filterType, 1, 2);

        switch (filterType)
        {
            case FilterType.Title:
                Assert.All(result, dto => Assert.Contains(dto.Books, b => b.Title.Contains(title!)));
                break;
            case FilterType.Author:
                Assert.All(result, dto => Assert.Contains(dto.Books, b => b.Authors.Any(a => a.FirstName == "Vladyslav")));
                break;
            case FilterType.ISBN:
                Assert.All(result, dto => Assert.Contains(dto.Books, b => b.ISBN == ISBN));
                break;
            case FilterType.All:
                Assert.Single(result); 
                break;
        }

        foreach (var orderDto in result)
        {
            foreach (var book in orderDto.Books)
            {
                Assert.NotNull(book.Title);
                Assert.NotNull(book.Authors);
                Assert.NotNull(book.Genres);
                Assert.True(book.Quantity > 0);
                Assert.True(book.Price >= 0);
               
            }
        }
    }


    [Fact]
    public async Task GetOrderDetails_ReturnsCorrectDetails()
    {
        var user = GenerateMockUser();
        var shoppingCart = GenerateMockShoppingCart();
        _orderRepositoryMock.Setup(repo => 
                repo.GetShoppingCartByUser(user, It.IsAny<CancellationToken>())).ReturnsAsync(shoppingCart);

        var result = await _orderService.GetOrderDetails(user);

        Assert.Equal(shoppingCart.CartItems.Sum(item => item.BookPrice * item.Quantity), result.TotalAmount);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task FindAsync_ReturnsCorrectOrder()
    {
        var orderId = Guid.NewGuid();
        var mockOrder = GenerateMockOrder();
        mockOrder.Id = orderId;
        _orderRepositoryMock.Setup(repo => repo.FindAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockOrder);

        var result = await _orderService.FindAsync(orderId);

        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedOrder()
    {
        var newOrder = GenerateMockOrder();
        _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(newOrder);

        var result = await _orderService.AddAsync(newOrder);

        Assert.Equal(newOrder, result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesOrder()
    {
        var orderToUpdate = GenerateMockOrder();
        _orderRepositoryMock.Setup(repo => 
                repo.UpdateAsync(orderToUpdate, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _orderService.UpdateAsync(orderToUpdate);

        _orderRepositoryMock.Verify(repo => 
            repo.UpdateAsync(orderToUpdate, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_RemovesOrder()
    {
        var orderToRemove = GenerateMockOrder();
       
        _orderRepositoryMock.Setup(repo =>
                repo.RemoveAsync(orderToRemove, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _orderService.RemoveAsync(orderToRemove);

        _orderRepositoryMock.Verify(repo =>
            repo.RemoveAsync(orderToRemove, It.IsAny<CancellationToken>()), Times.Once);
    }
}