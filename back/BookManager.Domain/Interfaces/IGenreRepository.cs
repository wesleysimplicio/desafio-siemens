using BookManager.Domain.Entities;

namespace BookManager.Domain.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre?> GetByNameAsync(string name);
    Task<Genre> AddAsync(Genre genre);
    Task<Genre> UpdateAsync(Genre genre);
    Task DeleteAsync(Genre genre);
    Task<bool> HasBooksAsync(int genreId);
}
