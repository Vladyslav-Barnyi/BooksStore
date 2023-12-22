using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<List<Author>> GetAuthorsAsync(int page, int pageSize, 
        CancellationToken ct = default);

    Task<Author?> FindAsync(Guid authorId, 
        CancellationToken ct = default);

    Task<Author> AddAuthor(Author author, 
        CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);

    Task UpdateAsync(Author author, CancellationToken ct = default);

    Task RemoveAsync(Author author, CancellationToken ct = default);
}