using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Person.Application.Interfaces.Repositories;
using Person.Domain.Enums;
using Person.Infrastructure.Data;
using Person.Infrastructure.Repositories;

namespace Person.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found");

        services.AddDbContext<PersonDbContext>(options =>
            options.UseNpgsql(connectionString,
                o => o.MapEnum<Gender>("gender")));

        services.AddScoped<IUnitOfWork, UnitOfWork<PersonDbContext>>();
        services.AddScoped<IPersonRepository, PersonRepository>();

        return services;
    }
}