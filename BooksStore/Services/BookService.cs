using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities;
using BooksStoreEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooksStore.Services;

public class BookService : IBookService
{

    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetBooksAsync(int page, int pageSize,
        CancellationToken ct = default)
    {
        //Try catch
        var books = await _bookRepository.GetBooksAsync(page, pageSize, ct);

        return books;
    }

    public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId, int page, int pageSize,
        CancellationToken ct = default)
    {
        var books = await _bookRepository.GetBooksByAuthorAsync(authorId, page, pageSize, ct);

        return books;
    }

    public async Task<List<Book>> GetBooksByGenreAsync(Guid genreId, int page, int pageSize,
        CancellationToken ct = default)
    {
        var books = await _bookRepository.GetBooksByGenreAsync(genreId, page, pageSize, ct);

        return books;
    }

    public async Task<Book?> FindAsync(Guid bookId,
        CancellationToken ct = default)
    {
        var book = await _bookRepository.FindAsync(bookId, ct);

        return book;
    }

    public async Task<Book> AddAsync(Book book,
        CancellationToken ct = default)
    {
        try
        {
            await _bookRepository.AddAsync(book, ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not create book");
            throw;
        }
        return book;
    }

    public async Task UpdateBookAsync(Book book,
        CancellationToken ct = default)
    {
        await _bookRepository.UpdateAsync(book, ct);
    }

    public async Task RemoveAsync(Book book,
        CancellationToken ct = default)
    {
        await _bookRepository.RemoveAsync(book,ct);
    }
}