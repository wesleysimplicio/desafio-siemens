using System.ComponentModel.DataAnnotations;

namespace BookManager.API.DTOs;

public record CreateBookDto(
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    string Title,

    [Required(ErrorMessage = "ISBN is required.")]
    [MaxLength(20, ErrorMessage = "ISBN cannot exceed 20 characters.")]
    string ISBN,

    [Range(1, 9999, ErrorMessage = "Published year must be between 1 and 9999.")]
    int PublishedYear,

    [Required(ErrorMessage = "AuthorId is required.")]
    int AuthorId,

    [Required(ErrorMessage = "GenreId is required.")]
    int GenreId,

    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    string? Description
);

public record UpdateBookDto(
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    string Title,

    [Required(ErrorMessage = "ISBN is required.")]
    [MaxLength(20, ErrorMessage = "ISBN cannot exceed 20 characters.")]
    string ISBN,

    [Range(1, 9999, ErrorMessage = "Published year must be between 1 and 9999.")]
    int PublishedYear,

    [Required(ErrorMessage = "AuthorId is required.")]
    int AuthorId,

    [Required(ErrorMessage = "GenreId is required.")]
    int GenreId,

    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    string? Description
);
