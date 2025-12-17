using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace BuildingBlocks.API.Extensions;

public static class SwaggerExtension
{
    private static readonly string AppName = Assembly.GetEntryAssembly()?.GetName().Name ?? "";

    extension(IServiceCollection services)
    {
        public void AddCustomSwagger()
        {
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
                options.SwaggerDoc("v1", new OpenApiInfo { Title = AppName, Version = "v1" });
            });
        }
    }

    extension(WebApplication app)
    {
        public void UseCustomSwagger()
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} v1"); });
        }
    }
}