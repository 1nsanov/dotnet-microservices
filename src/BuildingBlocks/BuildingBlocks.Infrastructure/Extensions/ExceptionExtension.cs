using Npgsql;

namespace BuildingBlocks.Infrastructure.Extensions;

internal static class ExceptionExtension
{
    public static bool IsConcurrentModifyException(this Exception exception)
    {
        return BaseIsConcurrentModifyException(exception) ||
               exception.InnerException != null && BaseIsConcurrentModifyException(exception.InnerException) ||
               exception.InnerException?.InnerException != null
               && BaseIsConcurrentModifyException(exception.InnerException.InnerException);

        bool BaseIsConcurrentModifyException(Exception innerException)
        {
            return innerException is PostgresException { SqlState: "40001" };
        }
    }
}