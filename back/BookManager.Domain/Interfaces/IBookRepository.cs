using BookManager.Domain.Entities;

namespace BookManager.Domain.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId);
    Task<IEnumerable<Book>> GetByGenreIdAsync(int genreId);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}
