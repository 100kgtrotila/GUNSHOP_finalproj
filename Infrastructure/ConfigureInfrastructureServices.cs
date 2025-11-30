using Application.Comments.Interfaces;
using Application.Comments.Queries; // <-- Додано
using Application.Customers.Interfaces;
using Application.Customers.Queries;
using Application.Orders.Interfaces;
using Application.Orders.Queries;
using Application.Weapons.Interfaces;
using Application.Weapons.Queries;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Queries;    // <-- Додано
using Infrastructure.Persistence.Repositories; // <-- Додано
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        // Існуюча реєстрація
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IWeaponRepository, WeaponRepository>();

        services.AddScoped<ICustomerQueries, CustomerQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<IWeaponQueries, WeaponQueries>();

        // V V V РЕЄСТРАЦІЯ НОВИХ СЕРВІСІВ V V V
        services.AddScoped<IProductCommentRepository, ProductCommentRepository>();
        services.AddScoped<IProductCommentQueries, ProductCommentQueries>();

        return services;
    }
}