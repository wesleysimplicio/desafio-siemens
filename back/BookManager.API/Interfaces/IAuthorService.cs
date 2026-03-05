using BookManager.API.DTOs;
using BookManager.API.ViewModels;

namespace BookManager.API.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorViewModel>> GetAllAsync();
    Task<AuthorViewModel> GetByIdAsync(int id);
    Task<AuthorViewModel> CreateAsync(CreateAuthorDto dto);
    Task<AuthorViewModel> UpdateAsync(int id, UpdateAuthorDto dto);
    Task DeleteAsync(int id);
}
