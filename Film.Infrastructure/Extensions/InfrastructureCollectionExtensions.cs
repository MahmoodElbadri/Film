using Film.Application.Interfaces;
using Film.Infrastructure.Data;
using Film.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Film.Infrastructure.Extensions;

public static class InfrastructureCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MovieDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMovieService, MovieService>(); 
        services.AddAutoMapper(typeof(InfrastructureCollectionExtensions).Assembly);
    }
}