using BooksStore.Repositories.Interfaces;
using BooksStore.Services.Interfaces;
using BooksStoreEntities.Entities;
using Serilog;

namespace BooksStore.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<List<Genre>> GetGenresAsync(int page, int pageSize, 
        CancellationToken ct = default)
    {
        var genres = await _genreRepository.GetGenresAsync(page, pageSize,ct);

        return genres;
    }

    public async Task<Genre?> FindAsync(Guid genreId, 
        CancellationToken ct = default)
    {
        var genre = await _genreRepository.FindAsync(genreId, ct);

        return genre;
    }

    public async Task<Genre> AddAsync(Genre genre, 
        CancellationToken ct = default)
    {
        try
        {
            await _genreRepository.AddAsync(genre, ct);
        }
        catch (Exception e)
        {
            Log.Error(e, "Could not create genre");
            throw;
        }

        return genre;
    }

    public async Task AddGenresToBookAsync(Book book, List<Guid> genreIds, 
        CancellationToken ct = default)
    {
        var genresToAdd = new List<Genre>();
        foreach (var genreId in genreIds)
        {
            var genre = await _genreRepository.FindAsync(genreId, ct);

            if (genre != null) genresToAdd.Add(genre);
        }
        book.Genres.AddRange(genresToAdd);
        await _genreRepository.SaveChangesAsync(ct);
    }

    public async Task RemoveGenresFromBookAsync(Book book, List<Guid> genreIds, 
        CancellationToken ct = default)
    {
        foreach (var genreId in genreIds)
        {
            var genre = await _genreRepository.FindAsync(genreId, ct);

            if (genre != null) book.Genres.Remove(genre);
        }

        await _genreRepository.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Genre genre, 
        CancellationToken ct = default)
    {
        await _genreRepository.UpdateAsync(genre, ct);
    }

    public async Task RemoveAsync(Genre genre, 
        CancellationToken ct = default)
    {
        await _genreRepository.RemoveAsync(genre,ct);
    }
}