using System.Data;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure;

public class UnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(
        Func<Task> action,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        int retryCount = 0,
        CancellationToken cancellationToken = default)
    {
        var retries = 0;

        while (true)
        {
            await using var transaction =
                await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

            try
            {
                await action();
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return;
            }
            catch (Exception exception)
            {
                _dbContext.ChangeTracker.Clear();
                await transaction.RollbackAsync(cancellationToken);

                if (exception.IsConcurrentModifyException() && retries < retryCount)
                {
                    retries++;
                    await Task.Delay(500, cancellationToken);
                    continue;
                }

                throw;
            }
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}