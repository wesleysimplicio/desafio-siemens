namespace BookManager.API.ViewModels;

public record GenreViewModel(int Id, string Name);

public record AuthorViewModel(int Id, string Name, string? Bio);

public record BookViewModel(
    int Id,
    string Title,
    string ISBN,
    int PublishedYear,
    string? Description,
    int AuthorId,
    string AuthorName,
    int GenreId,
    string GenreName
);
