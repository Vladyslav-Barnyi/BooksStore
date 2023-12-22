using BooksStoreEntities.Entities;

namespace BooksStore.Services.Interfaces;

public interface IBookService
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

    Task UpdateBookAsync(Book book,
        CancellationToken ct = default);

    Task RemoveAsync(Book book,
        CancellationToken ct = default);
}