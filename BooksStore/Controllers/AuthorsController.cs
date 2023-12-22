using BooksStore.Consumers.Author;
using BooksStore.Consumers.Base;
using BooksStore.Extensions;
using BooksStore.Filters;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthorsController(IAuthorService authorService, IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetAuthorResponse>>> GetAuthors([FromQuery] PaginationFilter filter,
        CancellationToken ct)
    {
        var authors = await authorService.GetAuthorsAsync(filter.Page, filter.PageSize, ct);

        var response = authors.Select(a => a.ToAuthorResponse());

        return Ok(response);
    }
 

    [HttpGet("{authorId}")]
    public async Task<ActionResult<Author>> GetAuthor(Guid authorId, 
        CancellationToken ct)
    {
        var author = await authorService.FindAsync(authorId, ct);
        if (author is null)
            return NotFound($"{nameof(Author)} with id: {authorId} was not Found");

        var response = author.ToAuthorResponse();

        return Ok(response);
    }
    
    
    [HttpPost]
    public async Task<ActionResult<Author>> AddAuthor(AddAuthorRequest r, 
        CancellationToken ct)
    {
        var authorModel = r.ToAuthor();
        var books = new List<Book>();
        foreach (var bookId in r.BookIds)
        {
            var book = await bookService.FindAsync(bookId, ct);
            if (book != null) books.Add(book);
        }
        authorModel.Books = books;

        var author = await authorService.AddAsync(authorModel, ct);
        var response = new CreateEntityResponse{ Id = author.Id};

        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> PatchAuthor(Guid authorId, 
        JsonPatchDocument<PatchAuthorRequest> r,
        CancellationToken ct)
    {
        var author = await authorService.FindAsync(authorId, ct);
        if (author is null)
            return NotFound($"{nameof(Author)} with id: {authorId} was not Found");

        var patch = author.ToPatchAuthorRequest();
        r.ApplyTo(patch, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        foreach (var bookId in patch.AddBookIds)
        {
            var bookToAdd = await bookService.FindAsync(bookId, ct);
            if (bookToAdd != null && !author.Books.Contains(bookToAdd))
                author.Books.Add(bookToAdd);
        }

        foreach (var bookId in patch.RemoveBookIds)
        {
            var bookToRemove = author.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove != null)
                author.Books.Remove(bookToRemove);
        }

        patch.Adapt(author);
        await authorService.UpdateAsync(author, ct);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAuthor(Guid authorId,
        CancellationToken ct)
    {
        var author = await authorService.FindAsync(authorId, ct);
        if (author is null)
            return NotFound($"{nameof(Author)} with id: {authorId} was not Found");

        await authorService.RemoveAsync(author, ct);

        return Ok();
    }
}