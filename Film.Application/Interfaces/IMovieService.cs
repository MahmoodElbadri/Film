using Film.Application.Dtos;
using Film.Domain.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Film.Application.Interfaces;

public interface IMovieService
{
    Task<PagedResult<MovieDto>> GetAllMoviesAsync(int pageNumber, int pageSize);
    Task<MovieDto?> GetMovieByIdAsync(int id);
    Task<MovieDto> UpdateAsync(int id, CreateMovieDto dto);
    Task<MovieDto> CreateAsync(CreateMovieDto dto);
    Task<bool> DeleteAsync(int id);
}
