using Film.Application.Dtos;
using Film.Domain.Entities;

namespace Film.Application.Interfaces;

public interface IEmailService
{
    Task SendNewMovieEmailAsync(MovieDto movie);
}
