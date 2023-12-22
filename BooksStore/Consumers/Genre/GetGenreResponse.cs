using BooksStore.Consumers.Book;
using BooksStore.Consumers.EntityModels;

namespace BooksStore.Consumers.Genre;

public class GetGenreResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<BookModel> BookModel { get; set; } = [];

}