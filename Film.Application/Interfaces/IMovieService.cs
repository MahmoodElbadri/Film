using Film.Application.Dtos;

namespace Film.Application.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync(int pageNumber, int pageSize);
    Task<MovieDto?> GetMovieByIdAsync(int id);
    Task<MovieDto> UpdateAsync(int id, CreateMovieDto dto);
    Task<MovieDto> CreateAsync(CreateMovieDto dto);
    Task<bool> DeleteAsync(int id);
}
