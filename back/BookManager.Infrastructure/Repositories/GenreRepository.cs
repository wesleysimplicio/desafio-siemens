using BookManager.Domain.Entities;
using BookManager.Domain.Interfaces;
using BookManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _context;

    public GenreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
        => await _context.Genres.AsNoTracking().OrderBy(g => g.Name).ToListAsync();

    public async Task<Genre?> GetByIdAsync(int id)
        => await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

    public async Task<Genre?> GetByNameAsync(string name)
        => await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => EF.Functions.ILike(g.Name, name));

    public async Task<Genre> AddAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre> UpdateAsync(Genre genre)
    {
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task DeleteAsync(Genre genre)
    {
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBooksAsync(int genreId)
        => await _context.Books.AnyAsync(b => b.GenreId == genreId);
}
