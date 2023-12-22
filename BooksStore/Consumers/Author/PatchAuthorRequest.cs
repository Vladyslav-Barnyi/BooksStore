namespace BooksStore.Consumers.Author;

public class PatchAuthorRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime? BirthDate { get; init; }
    public List<Guid> AddBookIds { get; } = [];
    public List<Guid> RemoveBookIds { get; } = [];
}