using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Person.Application.Services;
using Person.Application.Services.Interfaces;

namespace Person.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var autoMapperLicenseKey = configuration["AutoMapper:LicenseKey"] ?? string.Empty;
        
        services.AddAutoMapper(cfg => 
            cfg.LicenseKey = autoMapperLicenseKey,
            Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IPersonService, PersonService>();

        return services;
    }
}