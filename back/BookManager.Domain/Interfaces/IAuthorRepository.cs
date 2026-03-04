using BookManager.Domain.Entities;

namespace BookManager.Domain.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<Author?> GetByNameAsync(string name);
    Task<Author> AddAsync(Author author);
    Task<Author> UpdateAsync(Author author);
    Task DeleteAsync(Author author);
    Task<bool> HasBooksAsync(int authorId);
}
