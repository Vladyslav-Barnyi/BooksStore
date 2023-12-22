namespace BooksStoreEntities.Entities.Joins;

public class BooksGenres : BaseEntity
{
    public required Guid BookId { get; set; }

    public required Guid GenreId { get; set; }
}