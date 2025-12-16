using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Person.Application.Services;
using Person.Application.Services.Interfaces;

namespace Person.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => 
            cfg.LicenseKey = "", // TODO: Setup license key
            Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IPersonService, PersonService>();

        return services;
    }
}