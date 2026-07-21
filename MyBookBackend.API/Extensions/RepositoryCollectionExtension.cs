
using MyBookBackend.Repository;
using MyBookBackend.Repository.IRepository;

namespace MyBookBackend.API.Extensions;

public static class RepositoryCollectionExtension
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IBookRepository, BookRepository>();

        services.AddScoped<IAdminRepository, AdminRepository>();

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddScoped<IAuditRepository, AuditRepository>();

        return services;
    }
}