namespace MyBookBackend.API.Configurations;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:5173",
                        "https://findmybook.onrender.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}