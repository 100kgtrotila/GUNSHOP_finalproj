using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(ConfigureApplicationServices).Assembly));

        services.AddValidatorsFromAssembly(typeof(ConfigureApplicationServices).Assembly);

        return services;
    }
}