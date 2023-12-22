using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GenreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Genre>> GetGenresAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var genres = await _dbContext.Genres
            .Include(a => a.Books)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);

        return genres;
    }

    public async Task<Genre?> FindAsync(Guid genreId, CancellationToken ct = default)
    {
        var genre = await _dbContext.Genres
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == genreId, ct);

        return genre;
    }

    public async Task<Genre> AddAsync(Genre genre, CancellationToken ct = default)
    {
        await _dbContext.AddAsync(genre, ct);
        await _dbContext.SaveChangesAsync(ct);

        return genre;
    }


    public Task UpdateAsync(Genre genre, CancellationToken ct = default)
    {
        _dbContext.Update(genre);
        return SaveChangesAsync(ct);
    }

    public Task RemoveAsync(Genre genre, CancellationToken ct = default)
    {
        _dbContext.Remove(genre);
        return SaveChangesAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}