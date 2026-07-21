
using MyBookBackend.Service;
using MyBookBackend.Service.IServices;

namespace MyBookBackend.API.Extensions;

public static class ServiceCollectionExtensions

{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IBookService, BookService>();

        services.AddScoped<IAdminService, AdminService>();

        services.AddScoped<ICartService, CartService>();

        services.AddScoped<IAuditService, AuditService>();

        return services;
    }
}