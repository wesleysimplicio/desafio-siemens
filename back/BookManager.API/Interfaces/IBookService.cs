using BookManager.API.DTOs;
using BookManager.API.ViewModels;

namespace BookManager.API.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookViewModel>> GetAllAsync();
    Task<BookViewModel> GetByIdAsync(int id);
    Task<BookViewModel> CreateAsync(CreateBookDto dto);
    Task<BookViewModel> UpdateAsync(int id, UpdateBookDto dto);
    Task DeleteAsync(int id);
}
