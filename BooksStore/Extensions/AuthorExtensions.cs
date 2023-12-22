using BooksStore.Consumers.Author;
using BooksStore.Consumers.Book;
using BooksStore.Consumers.EntityModels;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class AuthorExtensions
{
    
    public static GetAuthorResponse ToAuthorResponse(this Author author)
    {
        var result = new GetAuthorResponse
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate,
            BookModel = author.Books.Select(b => new BookModel
            {
                Title = b.Title,
                PublicationDate = b.PublicationDate,
                Price = b.Price,
                ISBN = b.ISBN,
                Authors = b.Authors.Select(a=>a.ToAuthorModel()).ToList(),
                Genres = b.Genres.Select(g=>g.ToGenreModel()).ToList()
            })
        };
        return result;
    }

    public static Author ToAuthor(this AddAuthorRequest r)
    {

        var result = new Author
        {
            FirstName = r.FirstName,
            LastName = r.LastName,
            BirthDate = r.BirthDate
        };
        return result;
    }
    
    public static PatchAuthorRequest ToPatchAuthorRequest(
        this Author author)
    {
        var result = new PatchAuthorRequest
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate
        };

        return result;
    }

    public static void Adapt(
        this PatchAuthorRequest r, Author author)
    {
        if (r.FirstName is not null)
            author.FirstName = r.FirstName;
        
        if (r.LastName is not null)
            author.LastName = r.LastName;
        
        if (r.BirthDate is not null)
            author.BirthDate = (DateTime)r.BirthDate;
    }

    public static AuthorModel ToAuthorModel(this Author author)
    {
        var response = new AuthorModel
        {
            FirstName = author.FirstName,
            LastName = author.LastName
        };

        return response;
    }
}