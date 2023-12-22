using BooksStoreEntities.Entities;

namespace BooksStore.Services.Interfaces;

public interface IAuthorService
{
    Task<List<Author>> GetAuthorsAsync(int page, int pageSize,
        CancellationToken ct = default);

    Task<Author?> FindAsync(Guid authorId,
        CancellationToken ct = default);

    Task<Author> AddAsync(Author author,
        CancellationToken ct = default);
    
    Task AddAuthorsToBookAsync(Book book, List<Guid> authorsIds, 
        CancellationToken ct = default);
    
    Task RemoveAuthorsFromBookAsync(Book book, List<Guid> authorsIds, 
        CancellationToken ct = default);

    Task UpdateAsync(Author author,
        CancellationToken ct = default);

    Task RemoveAsync(Author author,
        CancellationToken ct = default);
}