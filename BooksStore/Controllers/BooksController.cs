using BooksStore.Consumers.Base;
using BooksStore.Consumers.Book;
using BooksStore.Extensions;
using BooksStore.Filters;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BooksController(IBookService bookService, IAuthorService authorService, IGenreService genreService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks([FromQuery] PaginationFilter filter,
        CancellationToken ct)
    {
        var books = await bookService.GetBooksAsync(filter.Page, filter.PageSize, ct);

        var response = books.Select(a => a.ToBookResponse());

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooksByAuthor([FromQuery] GetBookByAuthorRequest r,
        CancellationToken ct)
    {
        var author = await authorService.FindAsync(r.AuthorId, ct);
        if (author is null)
            return NotFound($"{nameof(Author)} with id: {r.AuthorId} was not Found");
        
        var books = await bookService.GetBooksByAuthorAsync(r.AuthorId, r.Page, r.PageSize, ct);

        var response = books.Select(a => a.ToBookResponse());

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooksByGenre([FromQuery] GetBookByGenreRequest r,
        CancellationToken ct)
    {
        var genre = await genreService.FindAsync(r.GenreId, ct);
        if (genre is null)
            return NotFound($"{nameof(Genre)} with id: {r.GenreId} was not Found");
        
        var books = await bookService.GetBooksByGenreAsync(r.GenreId, r.Page, r.PageSize, ct);

        var response = books.Select(a => a.ToBookResponse());

        return Ok(response);
    }
 

    [HttpGet("{bookId}")]
    public async Task<ActionResult<Book>> GetBook(Guid bookId, 
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(bookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {bookId} was not Found");

        var response = book.ToBookResponse();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> AddBook(AddBookRequest r, 
        CancellationToken ct)
    {
        var bookModel = r.ToBook();
        var authors = new List<Author>();
        var genres = new List<Genre>();

        foreach (var authorId in r.AuthorIds)
        {
            var author = await authorService.FindAsync(authorId, ct);
            if (author != null) authors.Add(author);
        }
        bookModel.Authors = authors;
        
        foreach (var genreId in r.GenreIds)
        {
            var genre = await genreService.FindAsync(genreId, ct);
            if (genre != null) genres.Add(genre);
        }
        bookModel.Genres = genres;

        var book = await bookService.AddAsync(bookModel, ct);
        var response = new CreateEntityResponse{Id = book.Id};

        return Ok(response);
        
    }
    
    [HttpPatch]
    public async Task<IActionResult> PatchBook(Guid bookId, 
        JsonPatchDocument<PatchBookRequest> r,
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(bookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {bookId} was not Found");

        var patch = book.ToPatchBookRequest();
        r.ApplyTo(patch, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        patch.Adapt(book);
        await bookService.UpdateBookAsync(book, ct);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteBook(Guid bookId,
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(bookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {bookId} was not Found");

        await bookService.RemoveAsync(book, ct);

        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageBookGenres(ManageBookGenresRequest r,
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(r.BookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {r.BookId} was not Found");

        if (r.AddGenres) {
            await genreService.AddGenresToBookAsync(book, r.GenreIds, ct);
        }
        else {
            await genreService.RemoveGenresFromBookAsync(book, r.GenreIds, ct);
        }

        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageBookAuthors(ManageBookAuthorsRequest r,
        CancellationToken ct)
    {
        var book = await bookService.FindAsync(r.BookId, ct);
        if (book is null)
            return NotFound($"{nameof(Book)} with id: {r.BookId} was not Found");
        
        if (r.AddAuthors) {
            await authorService.AddAuthorsToBookAsync(book, r.AuthorIds, ct);
        }
        else {
            await authorService.RemoveAuthorsFromBookAsync(book, r.AuthorIds, ct);
        }

        return Ok();
    }

}