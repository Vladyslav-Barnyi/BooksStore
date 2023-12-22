using BooksStore.Consumers.Author;
using BooksStore.Consumers.Book;
using BooksStore.Consumers.EntityModels;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class BookExtension
{
    public static GetBookResponse ToBookResponse(this Book book)
    {
        var response = new GetBookResponse
        {
            Title = book.Title,
            Price = book.Price,
            PublicationDate = book.PublicationDate,
            ISBN = book.ISBN,
            Authors = book.Authors.Select(a => a.ToAuthorModel()).ToList(),
            Genres = book.Genres.Select(g => g.ToGenreModel()).ToList()
        };

        return response;
    }

    public static Book ToBook(this AddBookRequest r)
    {
        var response = new Book
        {
            Title = r.Title,
            Price = r.Price,
            PublicationDate = r.PublicationDate,
            ISBN = r.ISBN
        };
        return response;
    }

    public static PatchBookRequest ToPatchBookRequest(this Book book)
    {
        var response = new PatchBookRequest
        {
            Title = book.Title,
            Price = book.Price,
            PublicationDate = book.PublicationDate,
            ISBN = book.ISBN
        };

        return response;
    }
    
    public static void Adapt(
        this PatchBookRequest r, Book book)
    {
        if (r.Title is not null)
            book.Title = r.Title;
        
        if (r.Price is not null)
            book.Price = (decimal)r.Price;
        
        if (r.PublicationDate is not null)
            book.PublicationDate = (DateTime)r.PublicationDate;
        
        if (r.ISBN is not null)
            book.ISBN = (int)r.ISBN;
    }
}