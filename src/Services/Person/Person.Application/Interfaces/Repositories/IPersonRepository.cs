using BuildingBlocks.Application.Interfaces.Repositories;

namespace Person.Application.Interfaces.Repositories;

public interface IPersonRepository : IRepositoryBase<Domain.Entities.Person>
{
}