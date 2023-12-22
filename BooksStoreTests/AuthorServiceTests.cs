using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly AuthorService _sut;

    public AuthorServiceTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _sut = new AuthorService(_authorRepositoryMock.Object);
    }
    
    private Author GenerateAuthorMock()
    {
        var author = new Author { Id = Guid.NewGuid(), FirstName = "Vladyslav",
            LastName = "Barnyi", BirthDate = DateTime.Today};

        return author;
    }

    private Book GenerateBookMock()
    {
        var book = new Book { Id = new Guid(), Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>()};

        return book;
    }
    
    [Fact]
    public async Task GetAuthorsAsync_ReturnsListOfAuthors()
    {
        var authors = new List<Author> { GenerateAuthorMock() };
        _authorRepositoryMock.Setup(repo => repo.GetAuthorsAsync(It.IsAny<int>(), 
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        var result = await _sut.GetAuthorsAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(authors.Count, result.Count);
    }
    
    [Fact]
    public async Task FindAsync_ExistingAuthorId_ReturnsAuthor()
    {
        var authorId = Guid.NewGuid();
        var expectedAuthor = GenerateAuthorMock();
        expectedAuthor.Id = authorId;
        _authorRepositoryMock.Setup(repo => 
                repo.FindAsync(authorId, It.IsAny<CancellationToken>())).ReturnsAsync(expectedAuthor);

        var result = await _sut.FindAsync(authorId);

        Assert.NotNull(result);
        Assert.Equal(expectedAuthor.Id, result.Id);
    }

    [Fact]
    public async Task AddAsync_NewAuthor_ReturnsAddedAuthor()
    {
        var newAuthor = GenerateAuthorMock();
        _authorRepositoryMock.Setup(repo => 
                repo.AddAuthor(newAuthor, It.IsAny<CancellationToken>())).ReturnsAsync(newAuthor);

        var result = await _sut.AddAsync(newAuthor);

        Assert.NotNull(result);
        _authorRepositoryMock.Verify(repo =>
            repo.AddAuthor(newAuthor, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidAuthor_UpdatesAuthor()
    {
        var authorToUpdate = GenerateAuthorMock();
        _authorRepositoryMock.Setup(repo => repo.UpdateAsync(authorToUpdate, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _sut.UpdateAsync(authorToUpdate);

        _authorRepositoryMock.Verify(repo => repo.UpdateAsync(authorToUpdate,
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_ValidAuthor_DeletesAuthor()
    {
        var authorToDelete = GenerateAuthorMock();
        _authorRepositoryMock.Setup(repo => repo.RemoveAsync(authorToDelete, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _sut.RemoveAsync(authorToDelete);

        _authorRepositoryMock.Verify(repo => repo.RemoveAsync(authorToDelete,
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    
    [Fact]
    public async Task RemoveAuthorsFromBookAsync_ValidData_BookAuthorsUpdated()
    {
        var book = GenerateBookMock();
        var authorsIds = new List<Guid>();

        await _sut.RemoveAuthorsFromBookAsync(book, authorsIds);

        Assert.Empty(book.Authors);
        _authorRepositoryMock.Verify(repo => 
            repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task RemoveAuthorsFromBookAsync_MultipleAuthors_AuthorsRemovedFromBook()
    {
        var initialAuthors = new List<Author>
        {
            new Author { Id = Guid.NewGuid(), BirthDate = DateTime.Today, FirstName = "Fabio", LastName = "Fara"},
            new Author { Id = Guid.NewGuid(), BirthDate = DateTime.Today, FirstName = "Diego", LastName = "Diaz"},
            new Author { Id = Guid.NewGuid(), BirthDate = DateTime.Today, FirstName = "Francisco", LastName = "Rubio"}
        };
        var book = GenerateBookMock();
        book.Authors = new List<Author>(initialAuthors); 
        var authorsIdsToRemove = new List<Guid> { initialAuthors[0].Id, initialAuthors[1].Id };

        foreach (var authorId in authorsIdsToRemove)
        {
            var author = initialAuthors.FirstOrDefault(a => a.Id == authorId);
            _authorRepositoryMock.Setup(repo => repo.FindAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);
        }

        await _sut.RemoveAuthorsFromBookAsync(book, authorsIdsToRemove);

        Assert.DoesNotContain(initialAuthors[0], book.Authors);
        Assert.DoesNotContain(initialAuthors[1], book.Authors);
        Assert.Contains(initialAuthors[2], book.Authors); 
        _authorRepositoryMock.Verify(repo =>
            repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAuthorsToBookAsync_MultipleAuthors_AuthorsAddedToBook()
    {
        var book = GenerateBookMock();

        var authorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var authors = authorsIds
            .Select(id => new Author { Id = id , BirthDate = DateTime.Today,
                FirstName = "Fabio", LastName = "Fara"}).ToList();

        foreach (var author in authors)
        {
            _authorRepositoryMock.Setup(repo => 
                    repo.FindAsync(author.Id, It.IsAny<CancellationToken>())).ReturnsAsync(author);
        }

        await _sut.AddAuthorsToBookAsync(book, authorsIds);

        Assert.Equal(authors.Count, book.Authors.Count);
        foreach (var author in authors)
        {
            Assert.Contains(author, book.Authors);
        }
        _authorRepositoryMock.Verify(repo => 
            repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}