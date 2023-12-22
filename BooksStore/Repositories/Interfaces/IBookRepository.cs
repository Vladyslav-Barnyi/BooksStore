using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetBooksAsync(int page, int pageSize,
        CancellationToken ct = default);

    Task<List<Book>> GetBooksByAuthorAsync(Guid authorId, int page, int pageSize,
        CancellationToken ct = default);

    Task<List<Book>> GetBooksByGenreAsync(Guid genreId, int page, int pageSize,
        CancellationToken ct = default);

    Task<Book?> FindAsync(Guid bookId,
        CancellationToken ct = default);

    Task<Book> AddAsync(Book book,
        CancellationToken ct = default);

    Task UpdateAsync(Book book, CancellationToken ct = default);

    Task RemoveAsync(Book book, CancellationToken ct = default);
    

}