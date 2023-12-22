using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object);
    }
    
    private Book GenerateBookMock()
    {
        var book = new Book { Id = new Guid(), Title = "lol", Price = 123,
            PublicationDate = DateTime.Today, ISBN = 123, Authors = new List<Author>()};

        return book;
    }
    
    [Fact]
    public async Task GetBooksAsync_ReturnsListOfBooks()
    {
        var books = new List<Book>();
        _bookRepositoryMock.Setup(repo =>
                repo.GetBooksAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var result = await _bookService.GetBooksAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(books.Count, result.Count);
    }

    [Fact]
    public async Task GetBooksByAuthorAsync_ReturnsBooksForAuthor()
    {
        var authorId = Guid.NewGuid();
        var books = new List<Book>();
        _bookRepositoryMock.Setup(repo => repo.GetBooksByAuthorAsync(authorId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var result = await _bookService.GetBooksByAuthorAsync(authorId, 1, 10);

        Assert.NotNull(result);
        Assert.Equal(books.Count, result.Count);
    }
    
    [Fact]
    public async Task GetBooksByGenreAsync_ReturnsBooksForGenre()
    {
        var genreId = Guid.NewGuid();
        var books = new List<Book>();
        _bookRepositoryMock.Setup(repo => 
                repo.GetBooksByGenreAsync(genreId, It.IsAny<int>(), 
                    It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(books);

        var result = await _bookService.GetBooksByGenreAsync(genreId, 1, 10);

        Assert.NotNull(result);
        Assert.Equal(books.Count, result.Count);
    }

    [Fact]
    public async Task FindAsync_ExistingBookId_ReturnsBook()
    {
        var bookId = Guid.NewGuid();
        var expectedBook = GenerateBookMock();
        expectedBook.Id = bookId;
        _bookRepositoryMock.Setup(repo => repo.FindAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBook);

        var result = await _bookService.FindAsync(bookId);

        Assert.NotNull(result);
        Assert.Equal(expectedBook.Id, result.Id);
    }

    [Fact]
    public async Task AddAsync_NewBook_ReturnsAddedBook()
    {
        var newBook = GenerateBookMock();

        _bookRepositoryMock.Setup(repo => repo.AddAsync(newBook, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newBook); 

        var result = await _bookService.AddAsync(newBook);

        Assert.NotNull(result);
        Assert.Equal(newBook.Id, result.Id); 
        _bookRepositoryMock.Verify(repo => repo.AddAsync(newBook, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task UpdateBookAsync_ValidBook_UpdatesBook()
    {
        var bookToUpdate = GenerateBookMock();
        _bookRepositoryMock.Setup(repo => repo.UpdateAsync(bookToUpdate, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _bookService.UpdateBookAsync(bookToUpdate);

        _bookRepositoryMock.Verify(repo => repo
            .UpdateAsync(bookToUpdate, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ValidBook_DeletesBook()
    {
        var bookToDelete = GenerateBookMock();
        _bookRepositoryMock.Setup(repo => repo.RemoveAsync(bookToDelete, 
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _bookService.RemoveAsync(bookToDelete);

        _bookRepositoryMock.Verify(repo => repo.RemoveAsync(bookToDelete, 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    
}