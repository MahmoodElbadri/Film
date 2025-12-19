using Film.Application.Dtos;
using Film.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Film.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(IMovieService _movieService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateMovie(CreateMovieDto dto)
    {
        var movie = await _movieService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMovies([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 5, [FromQuery] string searchTerm = "")
    {
        var movies = await _movieService.GetAllMoviesAsync(pageNumber, pageSize, searchTerm);
        return Ok(movies);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        return Ok(movie);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMovie(int id, CreateMovieDto dto)
    {
        await _movieService.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _movieService.DeleteAsync(id);
        return Ok();
    }
}
