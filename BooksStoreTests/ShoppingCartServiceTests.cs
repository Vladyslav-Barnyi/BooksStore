using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class ShoppingCartServiceTests
{
    private readonly Mock<IShoppingCartRepository> _shoppingCartRepositoryMock;
    private readonly ShoppingCartService _shoppingCartService;

    public ShoppingCartServiceTests()
    {
        _shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
        _shoppingCartService = new ShoppingCartService(_shoppingCartRepositoryMock.Object);
    }

    private ApplicationUser GenerateUserMock()
    {
        var user = new ApplicationUser
            { UserName = "Illuminati", Email = "illuminati@gmail.com", Password = "324fdsa" };
        return user;
    }

    private ShoppingCart GenerateShoppingCartMock()
    {
        var user = GenerateUserMock();
        var shoppingCart = new ShoppingCart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), ApplicationUser = user };

        return shoppingCart;
    }

    private CartItem GenerateCartItemMock()
    {
        var shoppingCart = GenerateShoppingCartMock();
        var book = new Book
        {
            Id = Guid.NewGuid(), Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>()
        };
        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(), Book = book, Quantity = 2, BookId = Guid.NewGuid(),
            BookPrice = 22, ShoppingCartId = Guid.NewGuid(), ShoppingCart = shoppingCart
        };

        return cartItem;
    }
    

    [Fact]
    public async Task FindShoppingCartAsync_ReturnsShoppingCart()
    {
        var shoppingCart = GenerateShoppingCartMock();

        _shoppingCartRepositoryMock.Setup(repo =>
                repo.FindShoppingCartAsync(shoppingCart.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(shoppingCart);

        var result = await _shoppingCartService.FindShoppingCartAsync(shoppingCart.Id);

        Assert.NotNull(result);
        Assert.Equal(shoppingCart, result);
    }

    [Fact]
    public async Task FindCartItemAsync_ReturnsCartItem()
    {
        var cartItem = GenerateCartItemMock();

        _shoppingCartRepositoryMock.Setup(repo => repo.FindCartItemAsync(cartItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);
        var result = await _shoppingCartService.FindCartItemAsync(cartItem.Id);

        Assert.NotNull(result);
        Assert.Equal(cartItem, result);
    }

    [Fact]
    public async Task GetShoppingCartByUserIdAsync_ReturnsShoppingCart()
    {
        var expectedShoppingCart = GenerateShoppingCartMock();
        _shoppingCartRepositoryMock.Setup(repo => repo.GetShoppingCartByUserIdAsync(expectedShoppingCart.UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedShoppingCart);

        var result = await _shoppingCartService.GetShoppingCartByUserIdAsync(expectedShoppingCart.UserId);

        Assert.NotNull(result);
        Assert.Equal(expectedShoppingCart, result);
    }

    [Fact]
    public async Task CreateShoppingCartAsync_CreatesShoppingCart()
    {
        var shoppingCart = GenerateShoppingCartMock();
        _shoppingCartRepositoryMock.Setup(repo => repo.CreateShoppingCartAsync(shoppingCart,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.CreateShoppingCartAsync(shoppingCart);

        _shoppingCartRepositoryMock.Verify(repo => repo.CreateShoppingCartAsync(shoppingCart,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddCartItemAsync_AddsCartItemAndReturnsIt()
    {
        var cartItem = GenerateCartItemMock();
        _shoppingCartRepositoryMock.Setup(repo => repo.AddCartItemAsync(cartItem,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem);

        var result = await _shoppingCartService.AddCartItemAsync(cartItem);

        Assert.Equal(cartItem, result);
        _shoppingCartRepositoryMock.Verify(repo => repo.AddCartItemAsync(cartItem,
            It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task UpdateCartItemAsync_UpdatesCartItem()
    {
        var cartItem = GenerateCartItemMock();

        _shoppingCartRepositoryMock.Setup(repo => repo.UpdateCartItemAsync(cartItem,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.UpdateCartItemAsync(cartItem);

        _shoppingCartRepositoryMock.Verify(repo => repo.UpdateCartItemAsync(cartItem,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCartItemAsync_DeletesCartItem()
    {
        var cartItem = GenerateCartItemMock();

        _shoppingCartRepositoryMock.Setup(repo => repo
                .FindShoppingCartAsync(cartItem.ShoppingCartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartItem.ShoppingCart);
        _shoppingCartRepositoryMock.Setup(repo => repo
                .SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.DeleteCartItemAsync(cartItem);

        Assert.DoesNotContain(cartItem, cartItem.ShoppingCart.CartItems);
        _shoppingCartRepositoryMock.Verify(repo => repo
            .SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddItemsToOrderAsync_AddsItemsToOrder()
    {
        var book = new Book
        {
            Id = Guid.NewGuid(), Title = "lol", Price = 100,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>()
        };

        var shoppingCart = GenerateShoppingCartMock();

        var order = new Order
        {
            Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>
            {
                GenerateCartItemMock(),
                GenerateCartItemMock()
            },
            TotalAmount = 400, User = shoppingCart.ApplicationUser, UserId = Guid.NewGuid()
        };
        
        var itemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var cartItems = itemIds.Select(id => new CartItem
        {
            Id = Guid.NewGuid(), Book = book, Quantity = 2, BookId = Guid.NewGuid(),
            BookPrice = 100, ShoppingCartId = Guid.NewGuid(), ShoppingCart = shoppingCart
        }).ToList();

        foreach (var item in cartItems)
        {
            _shoppingCartRepositoryMock.Setup(repo => repo.FindCartItemAsync(item.Id,
                It.IsAny<CancellationToken>())).ReturnsAsync(item);
        }

        _shoppingCartRepositoryMock.Setup(repo => repo.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.AddItemsToOrderAsync(order, itemIds);

        Assert.Equal(cartItems.Count, order.CartItems.Count);
        Assert.Equal(cartItems.Sum(i => i.BookPrice * i.Quantity), order.TotalAmount);
        _shoppingCartRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddItemsToOrderAsync_AddsItemsToOrderAndUpdatesTotalAmount()
    {
        var book = new Book { Id = Guid.NewGuid(), Title = "lol", Price = 100,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>() };

        var user = new ApplicationUser { UserName = "Illuminati", Email = "illuminati@gmail.com", Password = "324fdsa" };
        var shoppingCart = new ShoppingCart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), ApplicationUser = user };

        var order = new Order { Status = OrderStatus.Created, OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 0, User = user, UserId = Guid.NewGuid() };

        var itemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var cartItems = itemIds.Select(id => new CartItem 
        { 
            Id = id, Book = book, Quantity = 2, BookId = book.Id,
            BookPrice = 100, ShoppingCartId = shoppingCart.Id, ShoppingCart = shoppingCart
        }).ToList();

        foreach (var item in cartItems)
        {
            _shoppingCartRepositoryMock.Setup(repo => repo.FindCartItemAsync(item.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);
        }

        _shoppingCartRepositoryMock.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.AddItemsToOrderAsync(order, itemIds);

        Assert.Equal(cartItems.Count, order.CartItems.Count);
        foreach (var item in cartItems)
        {
            Assert.Contains(item, order.CartItems);
        }

        var expectedTotal = cartItems.Sum(i => i.BookPrice * i.Quantity);
        Assert.Equal(expectedTotal, order.TotalAmount);

        _shoppingCartRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveItemsFromOrderAsync_RemovesItemsFromOrder()
    {
        var book =  new Book { Id = Guid.NewGuid() , Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>()};
        var user = new ApplicationUser { UserName = "Illuminati", Email = "illuminati@gmail.com", Password = "324fdsa" };
        var shoppingCart = new ShoppingCart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), ApplicationUser = user };
        var order = new Order { Status = OrderStatus.Created , OrderDate = DateTime.Today,
            CartItems = new List<CartItem>(), TotalAmount = 300, User = user, UserId = Guid.NewGuid()};
        var initialItems = new List<CartItem>
        {
            new () { Id = Guid.NewGuid(), Book = book, Quantity = 1, BookId = Guid.NewGuid(),
                BookPrice = 100, ShoppingCartId = Guid.NewGuid(), ShoppingCart =  shoppingCart},
            new () { Id = Guid.NewGuid(), Book = book, Quantity = 1, BookId = Guid.NewGuid(),
                BookPrice = 100, ShoppingCartId = Guid.NewGuid(), ShoppingCart =  shoppingCart},
            new () { Id = Guid.NewGuid(), Book = book, Quantity = 1, BookId = Guid.NewGuid(),
                BookPrice = 100, ShoppingCartId = Guid.NewGuid(), ShoppingCart =  shoppingCart},
        };
        order.CartItems.AddRange(initialItems);
        var itemIdsToRemove = new List<Guid> { initialItems[0].Id, initialItems[1].Id };

        foreach (var itemId in itemIdsToRemove)
        {
            var item = initialItems.FirstOrDefault(i => i.Id == itemId);
            _shoppingCartRepositoryMock.Setup(repo => repo
                    .FindCartItemAsync(itemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);
        }

        _shoppingCartRepositoryMock.Setup(repo => repo
                .SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.RemoveItemsFromOrderAsync(order, itemIdsToRemove);

        Assert.DoesNotContain(initialItems[0], order.CartItems);
        Assert.DoesNotContain(initialItems[1], order.CartItems);
        Assert.Contains(initialItems[2], order.CartItems);
        Assert.Equal(100, order.TotalAmount);
        _shoppingCartRepositoryMock.Verify(repo => repo
            .SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CleanCartAfterOrderCreationAsync_ClearsShoppingCart()
    {
        var shoppingCart = GenerateShoppingCartMock();
        _shoppingCartRepositoryMock.Setup(repo => repo.GetShoppingCartByUserIdAsync(shoppingCart.UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shoppingCart);
        _shoppingCartRepositoryMock.Setup(repo => repo.UpdateShoppingCartAsync(shoppingCart,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _shoppingCartService.CleanCartAfterOrderCreationAsync(shoppingCart.UserId);

        Assert.Empty(shoppingCart.CartItems);
        _shoppingCartRepositoryMock.Verify(repo => repo.UpdateShoppingCartAsync(shoppingCart,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}