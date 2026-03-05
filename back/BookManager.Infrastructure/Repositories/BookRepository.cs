using BookManager.Domain.Entities;
using BookManager.Domain.Interfaces;
using BookManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Book?> GetByISBNAsync(string isbn)
        => await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => EF.Functions.ILike(b.ISBN, isbn));

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId)
        => await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Where(b => b.AuthorId == authorId)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<IEnumerable<Book>> GetByGenreIdAsync(int genreId)
        => await _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.GenreId == genreId)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}
