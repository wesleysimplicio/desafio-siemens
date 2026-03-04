using System.ComponentModel.DataAnnotations;

namespace BookManager.API.DTOs;

public record CreateAuthorDto(
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
    string Name,

    [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
    string? Bio
);

public record UpdateAuthorDto(
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
    string Name,

    [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
    string? Bio
);
