using BooksStore.Consumers.EntityModels;
using BooksStoreEntities.Entities;

namespace BooksStore.Consumers.Order;

public class GetOrderResponse
{
    public required Guid UserId { get; set; }
    public required decimal TotalAmount { get; set; }
    
    public required OrderStatus Status { get; set; }
    public required DateTime OrderDate { get; set; }

    public List<OrderBookModel> Books { get; set; } = [];

}
public class OrderBookModel
{
    public required string Title { get; set; }
    public required DateTime PublicationDate { get; set; }

    public required decimal Price { get; set; }
    public required int ISBN { get; set; }

    public required int Quantity { get; set; }

    public List<AuthorModel> Authors { get; set; } = [];
    public List<GenreModel> Genres { get; set; } = [];
}