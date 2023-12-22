using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Serilog;

namespace BooksStore.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }


    public async Task<List<Author>> GetAuthorsAsync(int page, int pageSize, 
        CancellationToken ct = default)
    {
        var authors = await _authorRepository.GetAuthorsAsync(page, pageSize, ct);

        return authors;
    }

    public async Task<Author?> FindAsync(Guid authorId, 
        CancellationToken ct = default)
    {
        var author = await _authorRepository.FindAsync(authorId, ct);

        return author;
    }

    public async Task<Author> AddAsync(Author author, 
        CancellationToken ct = default)
    {

        try
        {
            await _authorRepository.AddAuthor(author, ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not create book");
            throw;
        }
        return author;
    }

    public async Task AddAuthorsToBookAsync(Book book, List<Guid> authorsIds, 
        CancellationToken ct = default)
    {
        var authorsToAdd = new List<Author>();
        foreach (var authorId in authorsIds)
        {
            var author = await _authorRepository.FindAsync(authorId, ct);

            if (author != null) authorsToAdd.Add(author);
        }
        book.Authors.AddRange(authorsToAdd);
        await _authorRepository.SaveChangesAsync(ct);
    }

    public async Task RemoveAuthorsFromBookAsync(Book book, List<Guid> authorsIds, 
        CancellationToken ct = default)
    {
        foreach (var authorId in authorsIds)
        {
            var author = await _authorRepository.FindAsync(authorId, ct);

            if (author != null) book.Authors.Remove(author);
        }
        await _authorRepository.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Author author, 
        CancellationToken ct = default)
    {
        await _authorRepository.UpdateAsync(author, ct);
    }

    public async Task RemoveAsync(Author author, 
        CancellationToken ct = default)
    {
        await _authorRepository.RemoveAsync(author, ct);
    }
}