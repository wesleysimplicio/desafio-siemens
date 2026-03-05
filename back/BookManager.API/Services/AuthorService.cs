using BookManager.API.DTOs;
using BookManager.API.Interfaces;
using BookManager.API.ViewModels;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;

namespace BookManager.API.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IAuthorRepository repository, ILogger<AuthorService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<AuthorViewModel>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all authors.");
            var authors = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} author(s).", authors.Count());
            return authors.Select(ToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching authors.");
            throw;
        }
    }

    public async Task<AuthorViewModel> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching author ID {Id}.", id);
            var author = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Author), id);
            return ToViewModel(author);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Author ID {Id} not found.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching author ID {Id}.", id);
            throw;
        }
    }

    public async Task<AuthorViewModel> CreateAsync(CreateAuthorDto dto)
    {
        try
        {
            _logger.LogInformation("Creating author '{Name}'.", dto.Name);
            var existing = await _repository.GetByNameAsync(dto.Name);
            if (existing is not null)
                throw new ConflictException($"An author with name '{dto.Name}' already exists.");

            var author = new Author(dto.Name, dto.Bio);
            await _repository.AddAsync(author);
            _logger.LogInformation("Author '{Name}' created with ID {Id}.", author.Name, author.Id);
            return ToViewModel(author);
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict creating author: name '{Name}' already exists.", dto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating author '{Name}'.", dto.Name);
            throw;
        }
    }

    public async Task<AuthorViewModel> UpdateAsync(int id, UpdateAuthorDto dto)
    {
        try
        {
            _logger.LogInformation("Updating author ID {Id} to '{Name}'.", id, dto.Name);
            var author = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Author), id);

            var existing = await _repository.GetByNameAsync(dto.Name);
            if (existing is not null && existing.Id != id)
                throw new ConflictException($"An author with name '{dto.Name}' already exists.");

            author.Update(dto.Name, dto.Bio);
            await _repository.UpdateAsync(author);
            _logger.LogInformation("Author ID {Id} updated successfully.", id);
            return ToViewModel(author);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Author ID {Id} not found for update.", id);
            throw;
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict updating author ID {Id}: name '{Name}' already exists.", id, dto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating author ID {Id}.", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting author ID {Id}.", id);
            var author = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Author), id);

            var hasBooks = await _repository.HasBooksAsync(id);
            if (hasBooks)
                throw new BusinessException("Cannot delete an author that has books associated with them.");

            await _repository.DeleteAsync(author);
            _logger.LogInformation("Author ID {Id} deleted successfully.", id);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Author ID {Id} not found for deletion.", id);
            throw;
        }
        catch (BusinessException)
        {
            _logger.LogWarning("Cannot delete author ID {Id}: has associated books.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting author ID {Id}.", id);
            throw;
        }
    }

    private static AuthorViewModel ToViewModel(Author author)
        => new(author.Id, author.Name, author.Bio);
}
