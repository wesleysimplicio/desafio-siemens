using BookManager.API.DTOs;
using BookManager.API.Services;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookManager.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepoMock;
    private readonly Mock<IAuthorRepository> _authorRepoMock;
    private readonly Mock<IGenreRepository> _genreRepoMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _bookRepoMock = new Mock<IBookRepository>();
        _authorRepoMock = new Mock<IAuthorRepository>();
        _genreRepoMock = new Mock<IGenreRepository>();
        _service = new BookService(_bookRepoMock.Object, _authorRepoMock.Object, _genreRepoMock.Object);
    }

    private static Book CreateBookWithNavigation(
        string title, string isbn, int year, int authorId, int genreId,
        string authorName = "Author", string genreName = "Genre")
    {
        var author = new Author(authorName);
        var genre = new Genre(genreName);
        var book = new Book(title, isbn, year, authorId, genreId, null);

        // Use reflection to set navigation properties (private setters / EF-managed)
        typeof(Book).GetProperty(nameof(Book.Author))!
            .SetValue(book, author);
        typeof(Book).GetProperty(nameof(Book.Genre))!
            .SetValue(book, genre);

        return book;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            CreateBookWithNavigation("1984", "978-0451524935", 1949, 1, 1, "George Orwell", "Dystopia"),
            CreateBookWithNavigation("Brave New World", "978-0060850524", 1932, 2, 1, "Aldous Huxley", "Dystopia")
        };
        _bookRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(b => b.Title == "1984");
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookExists_ShouldReturnBook()
    {
        // Arrange
        var book = CreateBookWithNavigation("1984", "978-0451524935", 1949, 1, 1, "George Orwell", "Dystopia");
        _bookRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Title.Should().Be("1984");
        result.AuthorName.Should().Be("George Orwell");
        result.GenreName.Should().Be("Dystopia");
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        // Act
        Func<Task> act = () => _service.GetByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateBook()
    {
        // Arrange
        var dto = new CreateBookDto("Dune", "978-0441013593", 1965, 1, 1, "Epic sci-fi");
        var author = new Author("Frank Herbert");
        var genre = new Genre("Science Fiction");
        var savedBook = CreateBookWithNavigation("Dune", "978-0441013593", 1965, 1, 1, "Frank Herbert", "Science Fiction");

        _authorRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
        _genreRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);
        _bookRepoMock.Setup(r => r.GetByISBNAsync("978-0441013593")).ReturnsAsync((Book?)null);
        _bookRepoMock.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync((Book b) => b);
        _bookRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(savedBook);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Title.Should().Be("Dune");
        result.AuthorName.Should().Be("Frank Herbert");
        _bookRepoMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateISBN_ShouldThrowConflictException()
    {
        // Arrange
        var dto = new CreateBookDto("Duplicate", "978-0451524935", 2020, 1, 1, null);
        var existingBook = CreateBookWithNavigation("Original", "978-0451524935", 1949, 1, 1);
        var author = new Author("Some Author");
        var genre = new Genre("Some Genre");

        _authorRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
        _genreRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);
        _bookRepoMock.Setup(r => r.GetByISBNAsync("978-0451524935")).ReturnsAsync(existingBook);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*ISBN*");
    }

    [Fact]
    public async Task CreateAsync_WithInvalidAuthorId_ShouldThrowNotFoundException()
    {
        // Arrange
        var dto = new CreateBookDto("Book", "ISBN-001", 2020, 999, 1, null);
        _authorRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Author?)null);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task CreateAsync_WithInvalidGenreId_ShouldThrowNotFoundException()
    {
        // Arrange
        var dto = new CreateBookDto("Book", "ISBN-002", 2020, 1, 999, null);
        var author = new Author("Valid Author");
        _authorRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
        _genreRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Genre?)null);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task DeleteAsync_WhenBookExists_ShouldDelete()
    {
        // Arrange
        var book = CreateBookWithNavigation("Book to Delete", "ISBN-DEL", 2020, 1, 1);
        _bookRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _bookRepoMock.Verify(r => r.DeleteAsync(book), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        // Act
        Func<Task> act = () => _service.DeleteAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
