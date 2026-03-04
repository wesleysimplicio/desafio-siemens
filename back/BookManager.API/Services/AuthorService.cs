using BookManager.API.DTOs;
using BookManager.API.ViewModels;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;

namespace BookManager.API.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;

    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AuthorViewModel>> GetAllAsync()
    {
        var authors = await _repository.GetAllAsync();
        return authors.Select(ToViewModel);
    }

    public async Task<AuthorViewModel> GetByIdAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Author), id);

        return ToViewModel(author);
    }

    public async Task<AuthorViewModel> CreateAsync(CreateAuthorDto dto)
    {
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null)
            throw new ConflictException($"An author with name '{dto.Name}' already exists.");

        var author = new Author(dto.Name, dto.Bio);
        await _repository.AddAsync(author);

        return ToViewModel(author);
    }

    public async Task<AuthorViewModel> UpdateAsync(int id, UpdateAuthorDto dto)
    {
        var author = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Author), id);

        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null && existing.Id != id)
            throw new ConflictException($"An author with name '{dto.Name}' already exists.");

        author.Update(dto.Name, dto.Bio);
        await _repository.UpdateAsync(author);

        return ToViewModel(author);
    }

    public async Task DeleteAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Author), id);

        var hasBooks = await _repository.HasBooksAsync(id);
        if (hasBooks)
            throw new BusinessException("Cannot delete an author that has books associated with them.");

        await _repository.DeleteAsync(author);
    }

    private static AuthorViewModel ToViewModel(Author author)
        => new(author.Id, author.Name, author.Bio);
}
