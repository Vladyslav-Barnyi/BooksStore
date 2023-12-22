using BooksStore.Consumers.EntityModels;

namespace BooksStore.Consumers.User;

public class GetUserResponse
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required Guid? ShoppingCartId { get; set; }
    public List<OrderModel> Orders { get; set; } = [];
}