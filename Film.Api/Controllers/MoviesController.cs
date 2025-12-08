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
        await _movieService.CreateAsync(dto);
        return Ok();
    }

    [HttpGet]
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
    public async Task<IActionResult> UpdateMovie(int id, CreateMovieDto dto)
    {
        await _movieService.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _movieService.DeleteAsync(id);
        return Ok();
    }
}
