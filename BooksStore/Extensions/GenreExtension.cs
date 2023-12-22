using BooksStore.Consumers.Book;
using BooksStore.Consumers.EntityModels;
using BooksStore.Consumers.Genre;
using BooksStoreEntities.Entities;

namespace BooksStore.Extensions;

public static class GenreExtension
{
    public static GetGenreResponse ToGenreResponse(this Genre genre)
    {
        var response = new GetGenreResponse
        {
            Id = genre.Id,
            Name = genre.Name,
            BookModel = genre.Books.Select(b => new BookModel {
                Title = b.Title,
                PublicationDate = b.PublicationDate,
                Price = b.Price,
                ISBN = b.ISBN,
                Authors = b.Authors.Select(a=>a.ToAuthorModel()).ToList(),
                Genres = b.Genres.Select(g=>g.ToGenreModel()).ToList()
            })
        };
        
        return response;
    }

    public static PatchGenreRequest ToPatchGenreRequest(this Genre genre)
    {
        var response = new PatchGenreRequest {
            Name = genre.Name
        };

        return response;
    }
    
    public static Genre ToGenre(this AddGenreRequest r)
    {
        var response = new Genre {
            Name = r.Name
        };
        
        return response;
    }
    
    public static void Adapt(
        this PatchGenreRequest r, Genre genre)
    {
        if (r.Name is not null)
            genre.Name = r.Name;
    }

    public static GenreModel ToGenreModel(this Genre genre)
    {
        var response = new GenreModel {
            GenreName = genre.Name
        };
        
        return response;
    }
}