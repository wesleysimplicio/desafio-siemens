using BookManager.API.DTOs;
using BookManager.API.ViewModels;

namespace BookManager.API.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreViewModel>> GetAllAsync();
    Task<GenreViewModel> GetByIdAsync(int id);
    Task<GenreViewModel> CreateAsync(CreateGenreDto dto);
    Task<GenreViewModel> UpdateAsync(int id, UpdateGenreDto dto);
    Task DeleteAsync(int id);
}
