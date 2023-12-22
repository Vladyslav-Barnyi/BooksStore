using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class GenreServiceTests
{
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly GenreService _genreService;

    public GenreServiceTests()
    {
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _genreService = new GenreService(_genreRepositoryMock.Object);
    }

    private Genre GenerateGenreMock()
    {
        var genre = new Genre { Name = "Horror" };
        return genre;
    }

    [Fact]
    public async Task GetGenresAsync_ReturnsListOfGenres()
    {
        var genres = new List<Genre>();
        _genreRepositoryMock.Setup(repo =>
                repo.GetGenresAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(genres);

        var result = await _genreService.GetGenresAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(genres.Count, result.Count);
    }

    [Fact]
    public async Task FindAsync_ExistingGenreId_ReturnsGenre()
    {
        var genreId = Guid.NewGuid();
        var expectedGenre = GenerateGenreMock();
        expectedGenre.Id = genreId;
        _genreRepositoryMock.Setup(repo => repo.FindAsync(genreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGenre);

        var result = await _genreService.FindAsync(genreId);

        Assert.NotNull(result);
        Assert.Equal(expectedGenre.Id, result.Id);
    }

    [Fact]
    public async Task AddAsync_NewGenre_ReturnsAddedGenre()
    {
        var newGenre = GenerateGenreMock();
        _genreRepositoryMock.Setup(repo => repo.AddAsync(newGenre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newGenre);

        var result = await _genreService.AddAsync(newGenre);

        Assert.NotNull(result);
        _genreRepositoryMock.Verify(repo => repo.AddAsync(newGenre, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddGenresToBookAsync_MultipleGenres_GenresAddedToBook()
    {
        var book = new Book
        {
            Id = Guid.NewGuid(), Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Genres = new List<Genre>()
        };
        var genreIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var genres = new List<Genre>();
        var genre1 = GenerateGenreMock();
        genre1.Id = genreIds[0];
        var genre2 = GenerateGenreMock();
        genre2.Id = genreIds[1];
        genres.Add(genre1);
        genres.Add(genre2);

        foreach (var genre in genres)
        {
            _genreRepositoryMock.Setup(repo =>
                    repo.FindAsync(genre.Id, It.IsAny<CancellationToken>())).ReturnsAsync(genre);
        }

        await _genreService.AddGenresToBookAsync(book, genreIds);

        Assert.Equal(genres.Count, book.Genres.Count);
        foreach (var genre in genres)
        {
            Assert.Contains(genre, book.Genres);
        }

        _genreRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveGenresFromBookAsync_MultipleGenres_GenresRemovedFromBook()
    {
        var initialGenres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Horror" },
            new Genre { Id = Guid.NewGuid(), Name = "Fantasy" },
            new Genre { Id = Guid.NewGuid(), Name = "Fiction" }
        };
        var book = new Book
        {
            Id = Guid.NewGuid(), Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Genres = new List<Genre>(initialGenres)
        };
        var genreIdsToRemove = new List<Guid> { initialGenres[0].Id, initialGenres[1].Id };

        foreach (var genreId in genreIdsToRemove)
        {
            var genre = initialGenres.FirstOrDefault(a => a.Id == genreId);
            _genreRepositoryMock.Setup(repo => repo.FindAsync(genreId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(genre);
        }

        await _genreService.RemoveGenresFromBookAsync(book, genreIdsToRemove);

        Assert.DoesNotContain(initialGenres[0], book.Genres);
        Assert.DoesNotContain(initialGenres[1], book.Genres);
        Assert.Contains(initialGenres[2], book.Genres);
        _genreRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidGenre_UpdatesGenre()
    {
        var genreToUpdate = GenerateGenreMock();
        _genreRepositoryMock.Setup(repo => repo.UpdateAsync(genreToUpdate, 
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _genreService.UpdateAsync(genreToUpdate);

        _genreRepositoryMock.Verify(repo => repo.UpdateAsync(genreToUpdate, 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ValidGenre_DeletesGenre()
    {
        var genreToDelete = GenerateGenreMock();
        _genreRepositoryMock.Setup(repo => repo.RemoveAsync(genreToDelete, 
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _genreService.RemoveAsync(genreToDelete);

        _genreRepositoryMock.Verify(repo => repo.RemoveAsync(genreToDelete, 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}