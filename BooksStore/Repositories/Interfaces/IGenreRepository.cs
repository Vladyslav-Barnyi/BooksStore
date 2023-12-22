using BooksStoreEntities.Entities;

namespace BooksStore.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetGenresAsync(int page, int pageSize,
        CancellationToken ct = default);

    Task<Genre?> FindAsync(Guid genreId,
        CancellationToken ct = default);

    Task<Genre> AddAsync(Genre genre,
        CancellationToken ct = default);

    Task UpdateAsync(Genre genre,
        CancellationToken ct = default);

    Task RemoveAsync(Genre genre,
        CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
