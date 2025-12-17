using BuildingBlocks.Application.Interfaces.Repositories;
using BuildingBlocks.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : EntityBase, IAggregateRoot
{
    protected readonly DbContext DbContext;

    protected RepositoryBase(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);

        return Task.FromResult(entity);
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AnyAsync(entity => entity.Id == id, cancellationToken);
    }
}