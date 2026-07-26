using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace MyBookBackend.API.Configurations;

public static class RateLimitingConfiguration
{
    public static IServiceCollection AddRateLimitingConfiguration(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode =
                    StatusCodes.Status429TooManyRequests;

                await context.HttpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        isSuccess = false,
                        message = "Too many requests. Please try again later."
                    }, token);
            };

            // Books
            options.AddFixedWindowLimiter("BooksPolicy", limiter =>
            {
                limiter.PermitLimit = 100;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.QueueLimit = 0;
                limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            // Login
            options.AddSlidingWindowLimiter("LoginPolicy", limiter =>
            {
                limiter.PermitLimit = 5;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.SegmentsPerWindow = 6;
                limiter.QueueLimit = 0;
            });

            // Register
            options.AddSlidingWindowLimiter("RegisterPolicy", limiter =>
            {
                limiter.PermitLimit = 3;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.SegmentsPerWindow = 6;
                limiter.QueueLimit = 0;
            });
        });

        return services;
    }
}