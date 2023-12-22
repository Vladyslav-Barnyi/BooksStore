using BooksStore.Consumers.Book;
using BooksStore.Consumers.EntityModels;

namespace BooksStore.Consumers.Author;

public class GetAuthorResponse
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }
    
    public IEnumerable<BookModel> BookModel { get; set; } = [];

}
