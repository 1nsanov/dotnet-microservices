using Person.API.Middlewares;

namespace Person.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}