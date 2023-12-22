namespace BooksStore.Consumers.EntityModels;

public class BookModel
{
    public required string Title { get; set; }
    public required DateTime PublicationDate { get; set; }

    public required decimal Price { get; set; }
    public required int ISBN { get; set; }


    public List<AuthorModel> Authors { get; set; } = [];
    public List<GenreModel> Genres { get; set; } = [];
}
