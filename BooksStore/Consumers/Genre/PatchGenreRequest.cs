namespace BooksStore.Consumers.Genre;

public class PatchGenreRequest
{
    public string? Name { get; set; }
    public List<Guid> AddBookIds { get; } = [];
    public List<Guid> RemoveBookIds { get; } = [];
}