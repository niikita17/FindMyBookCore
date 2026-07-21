using Microsoft.EntityFrameworkCore;
using MyBookBackend.Domain.Data;

namespace MyBookBackend.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddApplicationDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}