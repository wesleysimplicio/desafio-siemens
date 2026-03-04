using BookManager.API.DTOs;
using BookManager.API.ViewModels;
using BookManager.Domain.Entities;
using BookManager.Domain.Exceptions;
using BookManager.Domain.Interfaces;

namespace BookManager.API.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;

    public GenreService(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GenreViewModel>> GetAllAsync()
    {
        var genres = await _repository.GetAllAsync();
        return genres.Select(ToViewModel);
    }

    public async Task<GenreViewModel> GetByIdAsync(int id)
    {
        var genre = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Genre), id);

        return ToViewModel(genre);
    }

    public async Task<GenreViewModel> CreateAsync(CreateGenreDto dto)
    {
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null)
            throw new ConflictException($"A genre with name '{dto.Name}' already exists.");

        var genre = new Genre(dto.Name);
        await _repository.AddAsync(genre);

        return ToViewModel(genre);
    }

    public async Task<GenreViewModel> UpdateAsync(int id, UpdateGenreDto dto)
    {
        var genre = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Genre), id);

        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null && existing.Id != id)
            throw new ConflictException($"A genre with name '{dto.Name}' already exists.");

        genre.Update(dto.Name);
        await _repository.UpdateAsync(genre);

        return ToViewModel(genre);
    }

    public async Task DeleteAsync(int id)
    {
        var genre = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Genre), id);

        var hasBooks = await _repository.HasBooksAsync(id);
        if (hasBooks)
            throw new BusinessException("Cannot delete a genre that has books associated with it.");

        await _repository.DeleteAsync(genre);
    }

    private static GenreViewModel ToViewModel(Genre genre)
        => new(genre.Id, genre.Name);
}
