using BuildingBlocks.Application.Interfaces.Repositories;

namespace Person.Application.Interfaces.Repositories;

public interface IPersonRepository : IRepositoryBase<Domain.Entities.Person>
{
    Task<bool> ExistsByEmailAsync(string email, Guid? excludePersonId = null, CancellationToken cancellationToken = default);
}