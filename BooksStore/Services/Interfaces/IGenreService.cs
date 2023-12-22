using BooksStoreEntities.Entities;

namespace BooksStore.Services.Interfaces;

public interface IGenreService
{
    Task<List<Genre>> GetGenresAsync(int page, int pageSize,
        CancellationToken ct = default);

    Task<Genre?> FindAsync(Guid genreId,
        CancellationToken ct = default);

    Task<Genre> AddAsync(Genre genre,
        CancellationToken ct = default);
    
    Task AddGenresToBookAsync(Book book, List<Guid> genreIds, 
        CancellationToken ct = default);
    
    Task RemoveGenresFromBookAsync(Book book, List<Guid> genreIds, 
        CancellationToken ct = default);

    Task UpdateAsync(Genre genre,
        CancellationToken ct = default);

    Task RemoveAsync(Genre genre,
        CancellationToken ct = default);
}