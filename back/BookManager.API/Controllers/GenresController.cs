using Asp.Versioning;
using BookManager.API.Common;
using BookManager.API.DTOs;
using BookManager.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    /// <summary>Returns all genres.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _service.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(genres));
    }

    /// <summary>Returns a genre by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var genre = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<object>.Ok(genre));
    }

    /// <summary>Creates a new genre.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateGenreDto dto)
    {
        var genre = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, ApiResponse<object>.Ok(genre, "Genre created successfully."));
    }

    /// <summary>Updates an existing genre.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreDto dto)
    {
        var genre = await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(genre, "Genre updated successfully."));
    }

    /// <summary>Deletes a genre by ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
