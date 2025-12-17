using BuildingBlocks.Infrastructure;
using Person.Application.Interfaces.Repositories;
using Person.Infrastructure.Data;

namespace Person.Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<Domain.Entities.Person>, IPersonRepository
{
    public PersonRepository(PersonDbContext context) : base(context)
    {
    }
}