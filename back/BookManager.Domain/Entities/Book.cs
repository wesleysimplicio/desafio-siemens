namespace BookManager.Domain.Entities;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int PublishedYear { get; private set; }
    public string ISBN { get; private set; } = string.Empty;

    public int AuthorId { get; private set; }
    public Author Author { get; private set; } = null!;

    public int GenreId { get; private set; }
    public Genre Genre { get; private set; } = null!;

    protected Book() { }

    public Book(string title, string isbn, int publishedYear, int authorId, int genreId, string? description = null)
    {
        SetTitle(title);
        SetISBN(isbn);
        SetPublishedYear(publishedYear);
        AuthorId = authorId;
        GenreId = genreId;
        Description = description?.Trim();
    }

    public void Update(string title, string isbn, int publishedYear, int authorId, int genreId, string? description = null)
    {
        SetTitle(title);
        SetISBN(isbn);
        SetPublishedYear(publishedYear);
        AuthorId = authorId;
        GenreId = genreId;
        Description = description?.Trim();
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title cannot be empty.", nameof(title));

        Title = title.Trim();
    }

    private void SetISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty.", nameof(isbn));

        ISBN = isbn.Trim();
    }

    private void SetPublishedYear(int year)
    {
        if (year < 1 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException("Invalid published year.", nameof(year));

        PublishedYear = year;
    }
}
