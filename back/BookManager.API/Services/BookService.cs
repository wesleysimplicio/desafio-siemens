using BookManager.API.DTOs;
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

    public BookService(
        IBookRepository repository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository)
    {
        _repository = repository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<BookViewModel>> GetAllAsync()
    {
        var books = await _repository.GetAllAsync();
        return books.Select(ToViewModel);
    }

    public async Task<BookViewModel> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Book), id);

        return ToViewModel(book);
    }

    public async Task<BookViewModel> CreateAsync(CreateBookDto dto)
    {
        _ = await _authorRepository.GetByIdAsync(dto.AuthorId)
            ?? throw new NotFoundException(nameof(Author), dto.AuthorId);

        _ = await _genreRepository.GetByIdAsync(dto.GenreId)
            ?? throw new NotFoundException(nameof(Genre), dto.GenreId);

        var existing = await _repository.GetByISBNAsync(dto.ISBN);
        if (existing is not null)
            throw new ConflictException($"A book with ISBN '{dto.ISBN}' already exists.");

        var book = new Book(dto.Title, dto.ISBN, dto.PublishedYear, dto.AuthorId, dto.GenreId, dto.Description);
        await _repository.AddAsync(book);

        var created = await _repository.GetByIdAsync(book.Id)
            ?? throw new NotFoundException(nameof(Book), book.Id);

        return ToViewModel(created);
    }

    public async Task<BookViewModel> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Book), id);

        _ = await _authorRepository.GetByIdAsync(dto.AuthorId)
            ?? throw new NotFoundException(nameof(Author), dto.AuthorId);

        _ = await _genreRepository.GetByIdAsync(dto.GenreId)
            ?? throw new NotFoundException(nameof(Genre), dto.GenreId);

        var existing = await _repository.GetByISBNAsync(dto.ISBN);
        if (existing is not null && existing.Id != id)
            throw new ConflictException($"A book with ISBN '{dto.ISBN}' already exists.");

        book.Update(dto.Title, dto.ISBN, dto.PublishedYear, dto.AuthorId, dto.GenreId, dto.Description);
        await _repository.UpdateAsync(book);

        var updated = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Book), id);

        return ToViewModel(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Book), id);

        await _repository.DeleteAsync(book);
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
}
