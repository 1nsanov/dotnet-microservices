using System.Net;
using System.Text.Json;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Person.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            DuplicateException duplicateEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = duplicateEx.Message,
                Details = new Dictionary<string, object>
                {
                    ["entityName"] = duplicateEx.EntityName,
                    ["propertyName"] = duplicateEx.PropertyName,
                    ["value"] = duplicateEx.Value
                }
            },
            DbUpdateException dbUpdateEx when IsUniqueConstraintViolation(dbUpdateEx) =>
                HandleUniqueConstraintViolation(dbUpdateEx),
            NotFoundException notFoundEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = notFoundEx.Message,
                Details = new Dictionary<string, object>
                {
                    ["entityName"] = notFoundEx.EntityName,
                    ["key"] = notFoundEx.Key
                }
            },
            ValidationException validationEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Validation error",
                Details = new Dictionary<string, object>
                {
                    ["errors"] = validationEx.Errors.Select(e => new
                    {
                        property = e.PropertyName,
                        message = e.ErrorMessage,
                        attemptedValue = e.AttemptedValue
                    })
                }
            },
            InvalidValueObjectException valueObjectEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = valueObjectEx.Message,
                Details = new Dictionary<string, object>
                {
                    ["valueObjectName"] = valueObjectEx.ValueObjectName,
                    ["reason"] = valueObjectEx.Reason
                }
            },
            InvalidEntityException entityEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = entityEx.Message,
                Details = new Dictionary<string, object>
                {
                    ["entityName"] = entityEx.EntityName,
                    ["reason"] = entityEx.Reason
                }
            },
            DomainException domainEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = domainEx.Message
            },
            _ => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred"
            }
        };

        response.StatusCode = errorResponse.StatusCode;

        if (errorResponse.StatusCode == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        var message = exception.InnerException?.Message ?? exception.Message;
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("UNIQUE KEY", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("IX_", StringComparison.OrdinalIgnoreCase);
    }

    private static ErrorResponse HandleUniqueConstraintViolation(DbUpdateException exception)
    {
        var message = exception.InnerException?.Message ?? exception.Message;

        var propertyName = ExtractPropertyNameFromMessage(message);
        var value = ExtractValueFromMessage(message);

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.Conflict,
            Message = string.IsNullOrEmpty(propertyName)
                ? "A record with the same unique value already exists"
                : $"A record with {propertyName} '{value}' already exists",
            Details = new Dictionary<string, object>
            {
                ["constraint"] = "unique_violation",
                ["propertyName"] = propertyName ?? "unknown",
                ["value"] = value ?? "unknown"
            }
        };
    }

    private static string? ExtractPropertyNameFromMessage(string message)
    {
        if (message.Contains("Email", StringComparison.OrdinalIgnoreCase))
            return "Email";
        if (message.Contains("Phone", StringComparison.OrdinalIgnoreCase))
            return "Phone";

        var indexMatch = System.Text.RegularExpressions.Regex.Match(message, @"IX_\w+_(\w+)");
        if (indexMatch.Success && indexMatch.Groups.Count > 1)
            return indexMatch.Groups[1].Value;

        return null;
    }

    private static string? ExtractValueFromMessage(string message)
    {
        var match = System.Text.RegularExpressions.Regex.Match(message, @"\(([^)]+)\)");
        if (match.Success && match.Groups.Count > 1)
            return match.Groups[1].Value;

        return null;
    }

    private class ErrorResponse
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public Dictionary<string, object>? Details { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }
}