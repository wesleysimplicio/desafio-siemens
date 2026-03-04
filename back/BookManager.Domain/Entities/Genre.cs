namespace BookManager.Domain.Entities;

public class Genre
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<Book> Books { get; private set; } = new List<Book>();

    protected Genre() { }

    public Genre(string name)
    {
        SetName(name);
    }

    public void Update(string name)
    {
        SetName(name);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Genre name cannot be empty.", nameof(name));

        Name = name.Trim();
    }
}
