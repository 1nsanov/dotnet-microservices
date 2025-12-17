using BuildingBlocks.API.Extensions;

namespace Person.API.Extensions;

public static class HostingExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplication ConfigureServices()
        {
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCustomSwagger();
            
            return builder.Build();
        }
    }

    extension(WebApplication app)
    {
        public WebApplication ConfigurePipeline()
        {
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
    }
}