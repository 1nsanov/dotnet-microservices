using System.Text.Json.Serialization;
using BuildingBlocks.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Person.Application;
using Person.Infrastructure;

namespace Person.API.Extensions;

public static class HostingExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplication ConfigureServices()
        {
            var environmentName = builder.Environment.EnvironmentName;
            builder.Configuration
                .AddJsonFile("appsettings.local.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.local.json", true, true)
                .AddEnvironmentVariables();
            
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    var enumConverter = new JsonStringEnumConverter(allowIntegerValues: false);
                    options.JsonSerializerOptions.Converters.Add(enumConverter);
                });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            
            builder.Services.AddCustomSwagger();
            
            return builder.Build();
        }
    }

    extension(WebApplication app)
    {
        public WebApplication ConfigurePipeline()
        {
            app.ApplyMigrations();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseCustomSwagger();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
            app.MapGet("api/ping", () => "pong");

            return app;
        }
        
        private void ApplyMigrations()
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            
            var shouldApplyMigrations = app.Environment.IsDevelopment() ||
                                        app.Configuration.GetValue("Database:AutoMigrate", false);

            if (!shouldApplyMigrations)
            {
                logger.LogInformation("Automatic migrations are disabled");
                return;
            }

            try
            {
                logger.LogInformation("Checking for pending migrations...");
                
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.PersonDbContext>();
                
                var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
                
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Found {Count} pending migration(s): {Migrations}", 
                        pendingMigrations.Count, 
                        string.Join(", ", pendingMigrations));
                    
                    logger.LogInformation("Applying migrations to database...");
                    dbContext.Database.Migrate();
                    
                    logger.LogInformation("Migrations applied successfully");
                }
                else
                {
                    logger.LogInformation("All migrations are already applied. Database is up to date");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while applying migrations to database");
                throw;
            }
        }
    }
}