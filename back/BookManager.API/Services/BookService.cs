using BookManager.API.DTOs;
using BookManager.API.Interfaces;
using BookManager.API.ViewModels;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;

namespace BookManager.API.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository repository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository,
        ILogger<BookService> logger)
    {
        _repository = repository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BookViewModel>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all books.");
            var books = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} book(s).", books.Count());
            return books.Select(ToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching books.");
            throw;
        }
    }

    public async Task<BookViewModel> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching book ID {Id}.", id);
            var book = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Book), id);
            return ToViewModel(book);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Book ID {Id} not found.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching book ID {Id}.", id);
            throw;
        }
    }

    public async Task<BookViewModel> CreateAsync(CreateBookDto dto)
    {
        try
        {
            _logger.LogInformation("Creating book '{Title}' (ISBN: {ISBN}).", dto.Title, dto.ISBN);
            var author = await _authorRepository.GetByIdAsync(dto.AuthorId)
                ?? throw new NotFoundException(nameof(Author), dto.AuthorId);

            var genre = await _genreRepository.GetByIdAsync(dto.GenreId)
                ?? throw new NotFoundException(nameof(Genre), dto.GenreId);

            var existing = await _repository.GetByISBNAsync(dto.ISBN);
            if (existing is not null)
                throw new ConflictException($"A book with ISBN '{dto.ISBN}' already exists.");

            var book = new Book(dto.Title, dto.ISBN, dto.PublishedYear, dto.AuthorId, dto.GenreId, dto.Description);
            await _repository.AddAsync(book);

            _logger.LogInformation("Book '{Title}' created with ID {Id}.", book.Title, book.Id);
            return ToViewModel(book, author.Name, genre.Name);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Not found during book creation (title: '{Title}').", dto.Title);
            throw;
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict creating book: ISBN '{ISBN}' already exists.", dto.ISBN);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating book '{Title}'.", dto.Title);
            throw;
        }
    }

    public async Task<BookViewModel> UpdateAsync(int id, UpdateBookDto dto)
    {
        try
        {
            _logger.LogInformation("Updating book ID {Id}.", id);
            var book = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Book), id);

            var author = await _authorRepository.GetByIdAsync(dto.AuthorId)
                ?? throw new NotFoundException(nameof(Author), dto.AuthorId);

            var genre = await _genreRepository.GetByIdAsync(dto.GenreId)
                ?? throw new NotFoundException(nameof(Genre), dto.GenreId);

            var existing = await _repository.GetByISBNAsync(dto.ISBN);
            if (existing is not null && existing.Id != id)
                throw new ConflictException($"A book with ISBN '{dto.ISBN}' already exists.");

            book.Update(dto.Title, dto.ISBN, dto.PublishedYear, dto.AuthorId, dto.GenreId, dto.Description);
            await _repository.UpdateAsync(book);

            _logger.LogInformation("Book ID {Id} updated successfully.", id);
            return ToViewModel(book, author.Name, genre.Name);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Not found during book update (ID: {Id}).", id);
            throw;
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict updating book ID {Id}: ISBN '{ISBN}' already exists.", id, dto.ISBN);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating book ID {Id}.", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting book ID {Id}.", id);
            var book = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Book), id);

            await _repository.DeleteAsync(book);
            _logger.LogInformation("Book ID {Id} deleted successfully.", id);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Book ID {Id} not found for deletion.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting book ID {Id}.", id);
            throw;
        }
    }

    private static BookViewModel ToViewModel(Book book)
        => new(
            book.Id,
            book.Title,
            book.ISBN,
            book.PublishedYear,
            book.Description,
            book.AuthorId,
            book.Author.Name,
            book.GenreId,
            book.Genre.Name
        );

    private static BookViewModel ToViewModel(Book book, string authorName, string genreName)
        => new(
            book.Id,
            book.Title,
            book.ISBN,
            book.PublishedYear,
            book.Description,
            book.AuthorId,
            authorName,
            book.GenreId,
            genreName
        );
}
