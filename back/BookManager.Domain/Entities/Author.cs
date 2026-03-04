namespace BookManager.Domain.Entities;

public class Author
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Bio { get; private set; }

    public ICollection<Book> Books { get; private set; } = new List<Book>();

    protected Author() { }

    public Author(string name, string? bio = null)
    {
        SetName(name);
        Bio = bio?.Trim();
    }

    public void Update(string name, string? bio = null)
    {
        SetName(name);
        Bio = bio?.Trim();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Author name cannot be empty.", nameof(name));

        Name = name.Trim();
    }
}
