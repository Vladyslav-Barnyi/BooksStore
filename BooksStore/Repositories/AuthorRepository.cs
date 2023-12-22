using BooksStore.Repositories.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<Author>> GetAuthorsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var authors = await _dbContext.Authors
            .Include(a => a.Books)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);

        return authors;
    }

    public async Task<Author?> FindAsync(Guid authorId, CancellationToken ct = default)
    {
        var author = await _dbContext.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == authorId,ct);

        return author;
    }

    public async Task<Author> AddAuthor(Author author, CancellationToken ct = default)
    {
        await _dbContext.AddAsync(author, ct);
        await _dbContext.SaveChangesAsync(ct);

        return author;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(Author author, CancellationToken ct = default)
    {
        _dbContext.Update(author);
        return SaveChangesAsync(ct);
    }

    public Task RemoveAsync(Author author, CancellationToken ct = default)
    {
        _dbContext.Remove(author);
        return SaveChangesAsync(ct);
    }
}