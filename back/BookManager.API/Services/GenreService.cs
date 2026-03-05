using BookManager.API.DTOs;
using BookManager.API.Interfaces;
using BookManager.API.ViewModels;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;

namespace BookManager.API.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IGenreRepository repository, ILogger<GenreService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<GenreViewModel>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all genres.");
            var genres = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} genre(s).", genres.Count());
            return genres.Select(ToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching genres.");
            throw;
        }
    }

    public async Task<GenreViewModel> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching genre ID {Id}.", id);
            var genre = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Genre), id);
            return ToViewModel(genre);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Genre ID {Id} not found.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching genre ID {Id}.", id);
            throw;
        }
    }

    public async Task<GenreViewModel> CreateAsync(CreateGenreDto dto)
    {
        try
        {
            _logger.LogInformation("Creating genre '{Name}'.", dto.Name);
            var existing = await _repository.GetByNameAsync(dto.Name);
            if (existing is not null)
                throw new ConflictException($"A genre with name '{dto.Name}' already exists.");

            var genre = new Genre(dto.Name);
            await _repository.AddAsync(genre);
            _logger.LogInformation("Genre '{Name}' created with ID {Id}.", genre.Name, genre.Id);
            return ToViewModel(genre);
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict creating genre: name '{Name}' already exists.", dto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating genre '{Name}'.", dto.Name);
            throw;
        }
    }

    public async Task<GenreViewModel> UpdateAsync(int id, UpdateGenreDto dto)
    {
        try
        {
            _logger.LogInformation("Updating genre ID {Id} to '{Name}'.", id, dto.Name);
            var genre = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Genre), id);

            var existing = await _repository.GetByNameAsync(dto.Name);
            if (existing is not null && existing.Id != id)
                throw new ConflictException($"A genre with name '{dto.Name}' already exists.");

            genre.Update(dto.Name);
            await _repository.UpdateAsync(genre);
            _logger.LogInformation("Genre ID {Id} updated successfully.", id);
            return ToViewModel(genre);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Genre ID {Id} not found for update.", id);
            throw;
        }
        catch (ConflictException)
        {
            _logger.LogWarning("Conflict updating genre ID {Id}: name '{Name}' already exists.", id, dto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating genre ID {Id}.", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting genre ID {Id}.", id);
            var genre = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Genre), id);

            var hasBooks = await _repository.HasBooksAsync(id);
            if (hasBooks)
                throw new BusinessException("Cannot delete a genre that has books associated with it.");

            await _repository.DeleteAsync(genre);
            _logger.LogInformation("Genre ID {Id} deleted successfully.", id);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Genre ID {Id} not found for deletion.", id);
            throw;
        }
        catch (BusinessException)
        {
            _logger.LogWarning("Cannot delete genre ID {Id}: has associated books.", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting genre ID {Id}.", id);
            throw;
        }
    }

    private static GenreViewModel ToViewModel(Genre genre)
        => new(genre.Id, genre.Name);
}
