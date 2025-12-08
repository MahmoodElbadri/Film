using Microsoft.Extensions.DependencyInjection;

namespace Film.Application.Extensions;

public static class ApplicationCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationCollectionExtensions).Assembly);
    }
}