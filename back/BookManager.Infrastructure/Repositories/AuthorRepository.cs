using BookManager.Domain.Entities;
using BookManager.Domain.Interfaces;
using BookManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
        => await _context.Authors.AsNoTracking().OrderBy(a => a.Name).ToListAsync();

    public async Task<Author?> GetByIdAsync(int id)
        => await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Author?> GetByNameAsync(string name)
        => await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => EF.Functions.ILike(a.Name, name));

    public async Task<Author> AddAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task DeleteAsync(Author author)
    {
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBooksAsync(int authorId)
        => await _context.Books.AnyAsync(b => b.AuthorId == authorId);
}
