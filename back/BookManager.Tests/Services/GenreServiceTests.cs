using BookManager.API.DTOs;
using BookManager.API.Services;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookManager.Tests.Services;

public class GenreServiceTests
{
    private readonly Mock<IGenreRepository> _repositoryMock;
    private readonly GenreService _service;

    public GenreServiceTests()
    {
        _repositoryMock = new Mock<IGenreRepository>();
        var loggerMock = new Mock<ILogger<GenreService>>();
        _service = new GenreService(_repositoryMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGenres()
    {
        // Arrange
        var genres = new List<Genre>
        {
            new("Fiction"),
            new("Science")
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(genres);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(g => g.Name == "Fiction");
        result.Should().Contain(g => g.Name == "Science");
    }

    [Fact]
    public async Task GetByIdAsync_WhenGenreExists_ShouldReturnGenre()
    {
        // Arrange
        var genre = new Genre("Fiction");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Fiction");
    }

    [Fact]
    public async Task GetByIdAsync_WhenGenreNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Genre?)null);

        // Act
        Func<Task> act = () => _service.GetByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task CreateAsync_WithUniqueName_ShouldCreateGenre()
    {
        // Arrange
        var dto = new CreateGenreDto("Fantasy");
        _repositoryMock.Setup(r => r.GetByNameAsync("Fantasy")).ReturnsAsync((Genre?)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre g) => g);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Fantasy");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        // Arrange
        var dto = new CreateGenreDto("Fiction");
        var existing = new Genre("Fiction");
        _repositoryMock.Setup(r => r.GetByNameAsync("Fiction")).ReturnsAsync(existing);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*Fiction*");
    }

    [Fact]
    public async Task UpdateAsync_WhenGenreExists_ShouldUpdateSuccessfully()
    {
        // Arrange
        var genre = new Genre("Old Name");
        var dto = new UpdateGenreDto("New Name");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);
        _repositoryMock.Setup(r => r.GetByNameAsync("New Name")).ReturnsAsync((Genre?)null);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre g) => g);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Name.Should().Be("New Name");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Genre>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenGenreNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Genre?)null);

        // Act
        Func<Task> act = () => _service.UpdateAsync(99, new UpdateGenreDto("Name"));

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenGenreHasNoBooks_ShouldDelete()
    {
        // Arrange
        var genre = new Genre("Empty Genre");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);
        _repositoryMock.Setup(r => r.HasBooksAsync(1)).ReturnsAsync(false);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(genre), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenGenreHasBooks_ShouldThrowBusinessException()
    {
        // Arrange
        var genre = new Genre("Used Genre");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(genre);
        _repositoryMock.Setup(r => r.HasBooksAsync(1)).ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*books associated*");
    }
}
