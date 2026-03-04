using System.ComponentModel.DataAnnotations;

namespace BookManager.API.DTOs;

public record CreateGenreDto(
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    string Name
);

public record UpdateGenreDto(
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    string Name
);
