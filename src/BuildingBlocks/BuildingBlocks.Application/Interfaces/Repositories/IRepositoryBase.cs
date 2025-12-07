using BuildingBlocks.Domain.Common;

namespace BuildingBlocks.Application.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
{
    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}