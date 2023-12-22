namespace BooksStoreEntities.Entities.Joins;

public class BooksAuthors : BaseEntity
{
    public required Guid BookId { get; set; }

    public required Guid AuthorId { get; set; }
}