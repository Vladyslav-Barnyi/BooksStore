using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BookRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetBooksAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var books = await _dbContext.Books
            .Include(a => a.Genres)
            .Include(b => b.Authors)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);

        return books;
    }

    public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId, int page, int pageSize,
        CancellationToken ct = default)
    {
        var books = await _dbContext.Books
            .Where(b => b.Authors.Any(a => a.Id == authorId))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return books;
    }

    public async Task<List<Book>> GetBooksByGenreAsync(Guid genreId, int page, int pageSize,
        CancellationToken ct = default)
    {
        var books = await _dbContext.Books
            .Where(b => b.Genres.Any(a => a.Id == genreId))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return books;
    }

    public async Task<Book?> FindAsync(Guid bookId, CancellationToken ct = default)
    {
        var book = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(b => b.Id == bookId,ct);

        return book;
    }

    public async Task<Book> AddAsync(Book book, CancellationToken ct = default)
    {
        await _dbContext.Books.AddAsync(book, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        return book;
    }

    private async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(Book book, CancellationToken ct = default)
    {
        _dbContext.Update(book);
        return SaveChangesAsync(ct);
    }

    public Task RemoveAsync(Book book, CancellationToken ct = default)
    {
        _dbContext.Remove(book);
        return SaveChangesAsync(ct);
    }
}