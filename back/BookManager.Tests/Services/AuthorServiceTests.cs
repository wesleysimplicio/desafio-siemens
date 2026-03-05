using BookManager.API.DTOs;
using BookManager.API.Services;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookManager.Tests.Services;

public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _repositoryMock;
    private readonly AuthorService _service;

    public AuthorServiceTests()
    {
        _repositoryMock = new Mock<IAuthorRepository>();
        var loggerMock = new Mock<ILogger<AuthorService>>();
        _service = new AuthorService(_repositoryMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAuthors()
    {
        // Arrange
        var authors = new List<Author>
        {
            new("George Orwell", "English novelist"),
            new("Aldous Huxley")
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(authors);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.Name == "George Orwell");
    }

    [Fact]
    public async Task GetByIdAsync_WhenAuthorExists_ShouldReturnAuthor()
    {
        // Arrange
        var author = new Author("George Orwell", "English novelist");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("George Orwell");
        result.Bio.Should().Be("English novelist");
    }

    [Fact]
    public async Task GetByIdAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Author?)null);

        // Act
        Func<Task> act = () => _service.GetByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task CreateAsync_WithUniqueName_ShouldCreateAuthor()
    {
        // Arrange
        var dto = new CreateAuthorDto("New Author", "Some bio");
        _repositoryMock.Setup(r => r.GetByNameAsync("New Author")).ReturnsAsync((Author?)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Author>()))
            .ReturnsAsync((Author a) => a);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Name.Should().Be("New Author");
        result.Bio.Should().Be("Some bio");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Author>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        // Arrange
        var dto = new CreateAuthorDto("George Orwell", null);
        var existing = new Author("George Orwell");
        _repositoryMock.Setup(r => r.GetByNameAsync("George Orwell")).ReturnsAsync(existing);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*George Orwell*");
    }

    [Fact]
    public async Task DeleteAsync_WhenAuthorHasNoBooks_ShouldDelete()
    {
        // Arrange
        var author = new Author("Lonely Author");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
        _repositoryMock.Setup(r => r.HasBooksAsync(1)).ReturnsAsync(false);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(author), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenAuthorHasBooks_ShouldThrowBusinessException()
    {
        // Arrange
        var author = new Author("Prolific Author");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);
        _repositoryMock.Setup(r => r.HasBooksAsync(1)).ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*books associated*");
    }

    [Fact]
    public async Task UpdateAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Author?)null);

        // Act
        Func<Task> act = () => _service.UpdateAsync(99, new UpdateAuthorDto("Name", null));

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
