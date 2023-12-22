using BooksStore.Consumers.Base;
using BooksStore.Consumers.Genre;
using BooksStore.Extensions;
using BooksStore.Filters;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GenresController(IGenreService genreService, IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetGenreResponse>>> GetGenres([FromQuery] PaginationFilter filter,
        CancellationToken ct)
    {
        var genres = await genreService.GetGenresAsync(filter.Page, filter.PageSize, ct);

        var response = genres.Select(a => a.ToGenreResponse());

        return Ok(response);
    }
    
    [HttpGet("{genreId}")]
    public async Task<ActionResult<Genre>> GetGenre(Guid genreId, 
        CancellationToken ct)
    {
        var genre = await genreService.FindAsync(genreId, ct);
        if (genre is null)
            return NotFound($"{nameof(Genre)} with id: {genreId} was not Found");

        var response = genre.ToGenreResponse();

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<Genre>> AddGenre(AddGenreRequest r, 
        CancellationToken ct)
    {
        var genreModel = r.ToGenre();
        var books = new List<Book>();
        foreach (var bookId in r.BookIds)
        {
            var book = await bookService.FindAsync(bookId, ct);
            if (book != null) books.Add(book);
        }
        genreModel.Books = books;

        var genre = await genreService.AddAsync(genreModel, ct);
        var response = new CreateEntityResponse{ Id = genre.Id};

        return Ok(response);
    }
        
    [HttpPatch]
    public async Task<IActionResult> PatchGenre(Guid genreId, 
        JsonPatchDocument<PatchGenreRequest> r,
        CancellationToken ct)
    {
        var genre = await genreService.FindAsync(genreId, ct);
        if (genre is null)
            return NotFound($"{nameof(Genre)} with id: {genreId} was not Found");

        var patch = genre.ToPatchGenreRequest();
        r.ApplyTo(patch, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        foreach (var bookId in patch.AddBookIds)
        {
            var bookToAdd = await bookService.FindAsync(bookId, ct);
            if (bookToAdd != null && !genre.Books.Contains(bookToAdd))
                genre.Books.Add(bookToAdd);
        }

        foreach (var bookId in patch.RemoveBookIds)
        {
            var bookToRemove = genre.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove != null)
                genre.Books.Remove(bookToRemove);
        }
        
        patch.Adapt(genre);
        await genreService.UpdateAsync(genre, ct);
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteGenre(Guid genreId,
        CancellationToken ct)
    {
        var genre = await genreService.FindAsync(genreId, ct);
        if (genre is null)
            return NotFound($"{nameof(Genre)} with id: {genreId} was not Found");

        await genreService.RemoveAsync(genre, ct);

        return Ok();
    }
}
